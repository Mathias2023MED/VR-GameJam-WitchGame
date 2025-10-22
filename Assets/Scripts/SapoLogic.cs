using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SapoLogic : MonoBehaviour
{
    // ---------- State names (must match your Animator) ----------
    [Header("Animator state names")]
    [SerializeField] string IdleState = "Idle";
    [SerializeField] string Walk1State = "Walk1";
    [SerializeField] string Walk2State = "Walk2";
    [SerializeField] string Walk3State = "Walk3";
    [SerializeField] string DrinkState = "Drink";
    [SerializeField] string BackOutState = "WalkingOut";
    [SerializeField] string DropKickState = "DropKick";
    [SerializeField] string HurricaneKickState = "HurricaneKick";
    [SerializeField] string RunningState = "Running";
    [SerializeField] string ShakingHeadState = "ShakingHead";

    [Header("Crossfade")]
    [SerializeField, Min(0f)] float crossfade = 0.1f;

    // ---------- Distance settings ----------
    [Header("Distance movement (manual/in-place)")]
    [Tooltip("Default meters/second when moving manually (not using root motion).")]
    public float moveSpeed = 1.6f;

    [Tooltip("Manual speeds for specific moves (used only when NOT using root motion).")]
    public float runningSpeed = 3.5f;
    public float hurricaneKickSpeed = 2.2f;

    [Tooltip("Which +Z should count as forward for manual movement. If null, uses this transform.")]
    public Transform forwardReference;

    // ---------- Drink / Teleport ----------
    [Header("Drink -> Teleport")]
    public bool teleportAfterDrink = false;
    public bool disableCharacterControllerOnTeleport = true;

    // ---------- Internals ----------
    Animator anim;
    CharacterController cc;
    Coroutine routine;

    Vector3 spawnPos;
    Quaternion spawnRot;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        if (!forwardReference) forwardReference = transform;

        spawnPos = transform.position;
        spawnRot = transform.rotation;

        // Default off: opt-in per action if needed
        anim.applyRootMotion = false;

        // Optional: warn if a state name is missing
        string[] expected = {
            IdleState, Walk1State, Walk2State, Walk3State, DrinkState, BackOutState,
            DropKickState, HurricaneKickState, RunningState, ShakingHeadState
        };
        foreach (var s in expected)
        {
            if (!string.IsNullOrEmpty(s) &&
                !anim.HasState(0, Animator.StringToHash("Base Layer." + s)))
            {
                Debug.LogWarning("[SapoLogic] Animator is missing state: " + s);
            }
        }
    }

    // ===================== PUBLIC API =====================

    // Distance-based walks (pass meters; choose root motion)
    public void Walk1_Distance(float meters, bool useRootMotion = false)
        => StartRoutine(Co_MoveDistance(Walk1State, meters, useRootMotion, moveSpeed));

    public void Walk2_Distance(float meters, bool useRootMotion = false)
        => StartRoutine(Co_MoveDistance(Walk2State, meters, useRootMotion, moveSpeed));

    public void Walk3_Distance(float meters, bool useRootMotion = false)
        => StartRoutine(Co_MoveDistance(Walk3State, meters, useRootMotion, moveSpeed));

    // Distance-based actions
    public void HurricaneKick_Distance(float meters, bool useRootMotion = false)
        => StartRoutine(Co_MoveDistance(HurricaneKickState, meters, useRootMotion, hurricaneKickSpeed));

    public void Running_Distance(float meters, bool useRootMotion = false)
        => StartRoutine(Co_MoveDistance(RunningState, meters, useRootMotion, runningSpeed));

    // One-shot actions (play once then return to Idle)
    public void PlayDrink()
        => StartRoutine(Co_PlayOnceReturn(DrinkState, MaybeTeleportHome));

    public void PlayWalkingOut()
        => StartRoutine(Co_PlayOnceReturn(BackOutState));

    public void PlayDropKick()
        => StartRoutine(Co_PlayOnceReturn(DropKickState));

    public void PlayShakingHeadOnce()
        => StartRoutine(Co_PlayOnceReturn(ShakingHeadState));

    // Generic helpers
    public void PlayOnce(string stateName)
        => StartRoutine(Co_PlayOnceReturn(stateName));

    public void PlayForSeconds(string stateName, float seconds)
        => StartRoutine(Co_PlayForSeconds(stateName, seconds));

    public void GoIdle()
        => CrossFade(IdleState);

    // ===================== COROUTINES =====================

    // Play a locomotion/action state until we moved "meters" on XZ plane, then Idle.
    IEnumerator Co_MoveDistance(string stateName, float meters, bool useRoot, float manualSpeed)
    {
        if (string.IsNullOrEmpty(stateName)) yield break;

        anim.applyRootMotion = useRoot;
        CrossFade(stateName);

        // Wait to actually enter the state
        yield return null;
        float safety = 2f;
        while (!IsIn(stateName) && (safety -= Time.deltaTime) > 0f) yield return null;

        Vector3 startPlanar = Planar(transform.position);
        float movedManual = 0f;

        while (true)
        {
            if (useRoot)
            {
                float planar = Vector3.Distance(Planar(transform.position), startPlanar);
                if (planar >= meters) break;
            }
            else
            {
                Vector3 dir = forwardReference.forward; dir.y = 0f; dir.Normalize();
                float step = manualSpeed * Time.deltaTime;
                if (cc && cc.enabled) cc.Move(dir * step);
                else transform.position += dir * step;

                movedManual += step;
                if (movedManual >= meters) break;
            }
            yield return null;
        }

        anim.applyRootMotion = false;
        CrossFade(IdleState);
        routine = null;
    }

    // Play a non-looping state; when nearly finished, run "after" and go Idle.
    IEnumerator Co_PlayOnceReturn(string stateName, System.Action after = null)
    {
        if (string.IsNullOrEmpty(stateName)) yield break;

        CrossFade(stateName);

        // Wait until we enter the state
        yield return null;
        while (!IsIn(stateName)) yield return null;

        // Wait until nearly finished
        while (IsIn(stateName) &&
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.98f)
        {
            yield return null;
        }

        if (after != null) after();
        CrossFade(IdleState);
        routine = null;
    }

    // Play for a fixed number of seconds; then Idle.
    IEnumerator Co_PlayForSeconds(string stateName, float seconds)
    {
        if (string.IsNullOrEmpty(stateName)) yield break;

        CrossFade(stateName);

        // Wait until we enter the state
        yield return null;
        while (!IsIn(stateName)) yield return null;

        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            yield return null;
        }

        CrossFade(IdleState);
        routine = null;
    }

    // ===================== HELPERS =====================

    void StartRoutine(IEnumerator co)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(co);
    }

    void CrossFade(string stateName)
    {
        anim.CrossFadeInFixedTime(stateName, crossfade, 0, 0f);
    }

    bool IsIn(string stateName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    static Vector3 Planar(Vector3 v)
    {
        return new Vector3(v.x, 0f, v.z);
    }

    void MaybeTeleportHome()
    {
        if (!teleportAfterDrink) return;

        if (disableCharacterControllerOnTeleport && cc) cc.enabled = false;
        transform.SetPositionAndRotation(spawnPos, spawnRot);
        if (disableCharacterControllerOnTeleport && cc) cc.enabled = true;
    }
}
