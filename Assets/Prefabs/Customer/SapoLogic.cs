using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SapoLogic : MonoBehaviour
{
    [Header("Animator state names (match your Animator)")]
    [SerializeField] string IdleState = "Idle";
    [SerializeField] string Walk1State = "Walk1";
    [SerializeField] string DrinkState = "Drink";

    [Header("Distance walking")]
    [Tooltip("Meters to move forward when Walk1 is played.")]
    public float walkDistance = 3f;

    [Tooltip("If your Walk1 is IN-PLACE, we move the transform at this speed.")]
    public float moveSpeed = 1.6f;

    [Tooltip("Use root motion for Walk1 (clip drives motion). If off, we move manually.")]
    public bool useRootMotion = false;

    [Tooltip("Optional: which way is 'forward' for walking. Defaults to this transform.")]
    public Transform forwardReference;

    [Header("Motion / Teleport")]
    public float crossfade = 0.1f;
    public bool disableCharacterControllerOnTeleport = true;

    Animator anim;
    Coroutine routine;
    CharacterController cc;

    Vector3 spawnPos;
    Quaternion spawnRot;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        if (!forwardReference) forwardReference = transform;

        spawnPos = transform.position;
        spawnRot = transform.rotation;

        // Default: keep root motion off unless we opt-in during Walk1
        anim.applyRootMotion = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartWalk1Distance();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartDrinkThenTeleport();
    }

    // ---------------- Public-style commands ----------------

    public void StartWalk1Distance()
    {
        StartRoutine(Co_Walk1_Distance_ThenIdle());
    }

    public void StartDrinkThenTeleport()
    {
        StartRoutine(Co_Drink_ThenTeleport());
    }

    // ---------------- Coroutines ----------------

    IEnumerator Co_Walk1_Distance_ThenIdle()
    {
        // Enter Walk1
        anim.applyRootMotion = useRootMotion;
        CrossFade(Walk1State);

        // Wait until we're actually in Walk1
        yield return null;
        while (!IsIn(Walk1State)) yield return null;

        float walked = 0f;
        Vector3 startPlanar = Planar(transform.position);

        while (true)
        {
            if (useRootMotion)
            {
                // Let animation drive; we just measure planar distance.
                float planar = Vector3.Distance(Planar(transform.position), startPlanar);
                if (planar >= walkDistance) break;
            }
            else
            {
                // Manual in-place movement.
                Vector3 dir = forwardReference.forward; dir.y = 0f; dir.Normalize();
                float step = moveSpeed * Time.deltaTime;
                if (cc && cc.enabled)
                    cc.Move(dir * step);
                else
                    transform.position += dir * step;

                walked += step;
                if (walked >= walkDistance) break;
            }
            yield return null;
        }

        // Return to Idle
        anim.applyRootMotion = false;
        CrossFade(IdleState);

        // Done – now idle for an 'uncertain' time until you press 2
        routine = null;
    }

    IEnumerator Co_Drink_ThenTeleport()
    {
        // If we're not in Idle yet (maybe mid-walk fade), wait
        while (!IsIn(IdleState) || anim.IsInTransition(0)) yield return null;

        CrossFade(DrinkState);

        // Wait until we are in Drink
        yield return null;
        while (!IsIn(DrinkState)) yield return null;

        // Wait until nearly finished (works for one-shot clip)
        while (IsIn(DrinkState) &&
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.98f)
            yield return null;

        // Teleport back to spawn
        if (disableCharacterControllerOnTeleport && cc) cc.enabled = false;
        transform.SetPositionAndRotation(spawnPos, spawnRot);
        if (disableCharacterControllerOnTeleport && cc) cc.enabled = true;

        CrossFade(IdleState);
        routine = null;
    }

    // ---------------- Helpers ----------------

    Vector3 Planar(Vector3 v) => new Vector3(v.x, 0f, v.z);

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
}
