using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class SapoAnimations : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField, Min(0f)] float crossfade = 0.1f;
    [SerializeField] Transform forwardReference;

    // Speeds for manual movement
    [SerializeField] float walkSpeed = 1.6f;
    [SerializeField] float runSpeed = 3.5f;
    [SerializeField] float hurricaneKickSpeed = 2.2f;

    Animator anim;
    Coroutine routine;

    // Per-action flags
    bool invertRootThisAction = false; // flips planar root motion (for opposite direction)

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (!forwardReference) forwardReference = transform;

        anim.applyRootMotion = false;
        anim.updateMode = AnimatorUpdateMode.Normal;
        anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        anim.speed = 1f;
    }

    // ---------------- PUBLIC API (generic) ----------------

    // Play a single clip/state once and optionally call `after` when finished.
    public void PlayOnce(string state, bool useRoot = false, Action after = null)
        => StartRoutine(Co_PlayOnceReturn(state, useRoot, after));

    // MoveDistance public wrapper: starts the internal coroutine and accepts an optional callback `after`.
    // useRoot true  -> distance from clip root motion (invertRoot flips it)
    // useRoot false -> manual movement at manualSpeed (backwards flips it)
    // preTurnYawDeg / preTurnTime -> smooth rotate before moving
    // restoreRotation / restoreTime -> smooth rotate back after moving
    public void MoveDistance(
        string state, float meters, bool useRoot, float manualSpeed,
        bool backwards = false, bool invertRoot = false,
        float preTurnYawDeg = 0f, float preTurnTime = 0f,
        bool restoreRotation = true, float restoreTime = 0f,
        Action after = null)
        => StartRoutine(Co_MoveDistance(
            state, meters, useRoot, manualSpeed, backwards, invertRoot,
            preTurnYawDeg, preTurnTime, restoreRotation, restoreTime, after));

    public void GoIdle() => CrossFade("Idle");

    // ---------------- CONVENIENCE WRAPPERS ----------------

    public void PlayDrink(Action after = null) => PlayOnce("Drink", useRoot: false, after);
    public void PlayShakingHead(Action after = null) => PlayOnce("ShakingHead", useRoot: false, after);
    public void PlayDropKick(Action after = null) => PlayOnce("DropKick", useRoot: true, after); // uses clip root motion

    public void Walk1_Distance(float meters, bool useRoot = false, Action after = null)
        => MoveDistance("Walk1", meters, useRoot, walkSpeed, false, false, 0f, 0f, true, 0f, after);

    public void Walk2_Distance(float meters, bool useRoot = false, Action after = null)
        => MoveDistance("Walk2", meters, useRoot, walkSpeed, false, false, 0f, 0f, true, 0f, after);

    public void Walk3_Distance(float meters, bool useRoot = false, Action after = null)
        => MoveDistance("Walk3", meters, useRoot, walkSpeed, false, false, 0f, 0f, true, 0f, after);

    public void Running_Distance(float meters, bool useRoot = false, Action after = null)
        => MoveDistance("Running", meters, useRoot, runSpeed, false, false, 0f, 0f, true, 0f, after);

    // Smooth pre-turn for WalkingOut: default 40 deg over 0.15s, restore over 0.15s
    public void WalkingOut_Distance(
        float meters, bool useRoot = true,
        float preTurnYawDeg = 40f, float preTurnTime = 0.15f,
        bool restoreRotation = true, float restoreTime = 0.15f,
        Action after = null)
        => MoveDistance(
            "WalkingOut", meters, useRoot, walkSpeed,
            backwards: true, invertRoot: false,
            preTurnYawDeg: preTurnYawDeg, preTurnTime: preTurnTime,
            restoreRotation: restoreRotation, restoreTime: restoreTime,
            after: after);

    // Hurricane kick:
    // useRoot = true  -> invert planar root motion (go backward with clip motion)
    // useRoot = false -> manual backward movement
    public void HurricaneKick_Distance(float meters, bool useRoot = false, Action after = null)
        => MoveDistance("HurricaneKick", meters, useRoot, hurricaneKickSpeed,
                        backwards: !useRoot, invertRoot: useRoot, after: after);

    // ---------------- COROUTINES ----------------

    IEnumerator Co_PlayOnceReturn(string state, bool useRoot, Action after = null)
    {
        if (string.IsNullOrEmpty(state)) yield break;

        invertRootThisAction = false;
        anim.applyRootMotion = useRoot;

        CrossFade(state);
        yield return null;

        if (!IsIn(state)) { PlayImmediate(state); yield return null; }
        if (!IsIn(state)) { anim.applyRootMotion = false; GoIdle(); routine = null; yield break; }

        while (IsIn(state) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.98f)
            yield return null;

        anim.applyRootMotion = false;

        // call the callback (if any) before going idle
        after?.Invoke();

        GoIdle();
        routine = null;
    }

    IEnumerator Co_MoveDistance(
        string state, float meters, bool useRoot, float manualSpeed,
        bool backwards, bool invertRoot,
        float preTurnYawDeg, float preTurnTime,
        bool restoreRotation, float restoreTime,
        Action after = null)
    {
        if (string.IsNullOrEmpty(state) || meters <= 0f) { GoIdle(); yield break; }

        Quaternion originalRot = transform.rotation;
        Quaternion targetRot = Quaternion.AngleAxis(preTurnYawDeg, Vector3.up) * originalRot;

        invertRootThisAction = useRoot && invertRoot;
        anim.applyRootMotion = useRoot;

        // Start the animation first so the blend hides the rotate tween
        CrossFade(state);
        yield return null;

        if (!IsIn(state)) { PlayImmediate(state); yield return null; }
        if (!IsIn(state))
        {
            anim.applyRootMotion = false;
            if (restoreRotation) transform.rotation = originalRot;
            GoIdle(); routine = null; yield break;
        }

        // Smooth pre-turn
        if (Mathf.Abs(preTurnYawDeg) > 0.001f && preTurnTime > 0f)
        {
            float t = 0f;
            while (t < preTurnTime)
            {
                float a = t / preTurnTime;
                a = a * a * (3f - 2f * a); // smoothstep
                transform.rotation = Quaternion.Slerp(originalRot, targetRot, a);

                // keep non-looping clips alive during the tween
                var info0 = anim.GetCurrentAnimatorStateInfo(0);
                if (!info0.loop && info0.normalizedTime >= 0.98f) PlayImmediate(state);

                t += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRot;
        }
        else
        {
            // Instant pre-turn if time is zero (you can set time > 0 to smooth)
            if (Mathf.Abs(preTurnYawDeg) > 0.001f) transform.rotation = targetRot;
        }

        // Start measuring distance AFTER the turn
        Vector3 startPlanar = Planar(transform.position);
        float movedManual = 0f;

        while (true)
        {
            if (useRoot)
            {
                float planar = Vector3.Distance(Planar(transform.position), startPlanar);
                if (planar >= meters) break;

                // keep non-looping clips alive
                var info = anim.GetCurrentAnimatorStateInfo(0);
                if (!info.loop && info.normalizedTime >= 0.98f) PlayImmediate(state);
            }
            else
            {
                Vector3 dir = forwardReference ? forwardReference.forward : transform.forward;
                dir.y = 0f; dir.Normalize();
                if (backwards) dir = -dir;

                float step = Mathf.Min(manualSpeed * Time.deltaTime, meters - movedManual);
                transform.position += dir * step;
                movedManual += step;

                // keep non-looping clips alive visually
                var info = anim.GetCurrentAnimatorStateInfo(0);
                if (!info.loop && info.normalizedTime >= 0.98f) PlayImmediate(state);

                if (movedManual >= meters - 1e-3f) break;
            }
            yield return null;
        }

        anim.applyRootMotion = false;
        invertRootThisAction = false;

        // Smooth restore
        if (restoreRotation)
        {
            if (restoreTime > 0f)
            {
                Quaternion from = transform.rotation;
                float t = 0f;
                while (t < restoreTime)
                {
                    float a = t / restoreTime;
                    a = a * a * (3f - 2f * a); // smoothstep
                    transform.rotation = Quaternion.Slerp(from, originalRot, a);
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            transform.rotation = originalRot;
        }

        // call the callback (if any) before going idle
        after?.Invoke();

        GoIdle();
        routine = null;
    }

    // ---------------- HELPERS ----------------

    void StartRoutine(IEnumerator co)
    { if (routine != null) StopCoroutine(routine); routine = StartCoroutine(co); }

    // Accepts "State" or "SubSM/State"
    void CrossFade(string s)
    {
        string p = ToPath(s);
        if (p == null) return;
        anim.CrossFadeInFixedTime(p, crossfade, 0, 0f);
        anim.Update(0f);
    }

    void PlayImmediate(string s)
    {
        string p = ToPath(s);
        if (p == null) return;
        anim.Play(p, 0, 0f);
        anim.Update(0f);
    }

    bool IsIn(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        var info = anim.GetCurrentAnimatorStateInfo(0);
        string p = ToPath(s);
        return info.IsName(s) || info.IsName("Base Layer." + s) || (p != null && info.IsName(p));
    }

    static string ToPath(string s)
    {
        if (string.IsNullOrEmpty(s)) return null;
        return s.Contains("/") ? s.Replace('/', '.') : s;
    }

    static Vector3 Planar(Vector3 v) => new Vector3(v.x, 0f, v.z);

    void OnAnimatorMove()
    {
        if (!anim || !anim.applyRootMotion) return;

        Vector3 delta = anim.deltaPosition;
        Quaternion dRot = anim.deltaRotation;

        // Invert planar root motion for opposite-direction clips when requested
        if (invertRootThisAction)
        {
            Vector3 fwd = forwardReference ? forwardReference.forward : transform.forward;
            fwd.y = 0f; fwd.Normalize();

            Vector3 planar = new Vector3(delta.x, 0f, delta.z);
            float along = Vector3.Dot(planar, fwd);
            Vector3 alongVec = along * fwd;
            Vector3 sideVec = planar - alongVec;

            Vector3 invertedPlanar = (-alongVec) + sideVec;
            delta.x = invertedPlanar.x;
            delta.z = invertedPlanar.z;
        }

        transform.position += delta;
        transform.rotation *= dRot;
    }
}
