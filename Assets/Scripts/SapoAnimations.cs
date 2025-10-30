using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SapoAnimations : MonoBehaviour
{
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

    [Header("Distance movement (manual/in-place)")]
    public float moveSpeed = 1.6f;
    public float runningSpeed = 3.5f;
    public float hurricaneKickSpeed = 2.2f;
    public Transform forwardReference;

    Animator anim;
    Coroutine routine;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (!forwardReference) forwardReference = transform;

        // default off; we toggle per-action
        anim.applyRootMotion = false;

        // Optional sanity check
        string[] expected = {
            IdleState, Walk1State, Walk2State, Walk3State, DrinkState, BackOutState,
            DropKickState, HurricaneKickState, RunningState, ShakingHeadState
        };
        foreach (var s in expected)
            if (!string.IsNullOrEmpty(s) &&
                !anim.HasState(0, Animator.StringToHash("Base Layer." + s)))
                Debug.LogWarning("[SapoAnimations] Animator is missing state: " + s);
    }

    // ---------- Play Anmimation methods ----------
    public void Walk1_Distance(float m, bool useRoot = false)
        => StartRoutine(Co_MoveDistance(Walk1State, m, useRoot, moveSpeed));
    public void Walk2_Distance(float m, bool useRoot = false)
        => StartRoutine(Co_MoveDistance(Walk2State, m, useRoot, moveSpeed));
    public void Walk3_Distance(float m, bool useRoot = false)
        => StartRoutine(Co_MoveDistance(Walk3State, m, useRoot, moveSpeed));
    public void HurricaneKick_Distance(float m, bool useRoot = false)
        => StartRoutine(Co_MoveDistance(HurricaneKickState, m, useRoot, hurricaneKickSpeed));
    public void Running_Distance(float m, bool useRoot = false)
        => StartRoutine(Co_MoveDistance(RunningState, m, useRoot, runningSpeed));
    public void WalkingOut_Distance(float m, bool useRoot = true)
    => StartRoutine(Co_MoveDistance(BackOutState, m, useRoot, moveSpeed, backwards: true));


    public void PlayDrink() => StartRoutine(Co_PlayOnceReturn(DrinkState));
    public void PlayDropKick() => StartRoutine(Co_PlayOnceReturn(DropKickState));
    public void PlayShakingHead() => StartRoutine(Co_PlayOnceReturn(ShakingHeadState));

    public void PlayOnce(string state) => StartRoutine(Co_PlayOnceReturn(state));
    public void PlayForSeconds(string state, float s) => StartRoutine(Co_PlayForSeconds(state, s));
    public void GoIdle() => CrossFade(IdleState);

    // ---------- Coroutines ----------
    IEnumerator Co_MoveDistance(string stateName, float meters, bool useRoot, float manualSpeed, bool backwards = false)
    {
        if (string.IsNullOrEmpty(stateName)) yield break;

        anim.applyRootMotion = useRoot;
        CrossFade(stateName);

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
                Vector3 dir = forwardReference ? forwardReference.forward : transform.forward;
                if (backwards) dir = -dir;   // <<— go backwards for WalkingOut
                dir.y = 0f; dir.Normalize();

                float step = manualSpeed * Time.deltaTime;
                transform.position += dir * step;

                movedManual += step;
                if (movedManual >= meters) break;
            }
            yield return null;
        }

        anim.applyRootMotion = false;
        CrossFade(IdleState);
        routine = null;
    }


    IEnumerator Co_PlayOnceReturn(string stateName, System.Action after = null)
    {
        if (string.IsNullOrEmpty(stateName)) yield break;

        CrossFade(stateName);
        yield return null;
        while (!IsIn(stateName)) yield return null;

        while (IsIn(stateName) &&
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.98f)
            yield return null;

        after?.Invoke();
        CrossFade(IdleState);
        routine = null;
    }

    IEnumerator Co_PlayForSeconds(string stateName, float seconds)
    {
        if (string.IsNullOrEmpty(stateName)) yield break;

        CrossFade(stateName);
        yield return null;
        while (!IsIn(stateName)) yield return null;

        float t = 0f;
        while (t < seconds) { t += Time.deltaTime; yield return null; }

        CrossFade(IdleState);
        routine = null;
    }

    // ---------- Helpers ----------
    void StartRoutine(IEnumerator co)
    { if (routine != null) StopCoroutine(routine); routine = StartCoroutine(co); }

    void CrossFade(string s) => anim.CrossFadeInFixedTime(s, crossfade, 0, 0f);
    bool IsIn(string s) => anim.GetCurrentAnimatorStateInfo(0).IsName(s);
    static Vector3 Planar(Vector3 v) => new Vector3(v.x, 0f, v.z);

    // Ensure root-motion moves/rotates the transform when enabled
    void OnAnimatorMove()
    {
        if (!anim || !anim.applyRootMotion) return;
        transform.position += anim.deltaPosition;
        transform.rotation *= anim.deltaRotation;
    }
}

