using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SapoAnimController : MonoBehaviour
{
    [Header("State names must match your Animator")]
    [SerializeField] string IdleState = "Idle";
    [SerializeField] string Walk1State = "Walk1";
    [SerializeField] string Walk2State = "Walk2";
    [SerializeField] string Walk3State = "Walk3";
    [SerializeField] string DrinkState = "Drink";
    [SerializeField] string BackOutState = "WalkingOut";

    [SerializeField] float fade = 0.1f;

    Animator anim;
    Coroutine routine;

    void Awake() { anim = GetComponent<Animator>(); }

    // Public calls from your game code:
    public void PlayWalk1(float seconds) => PlayFor(Walk1State, seconds);
    public void PlayWalk2(float seconds) => PlayFor(Walk2State, seconds);
    public void PlayWalk3(float seconds) => PlayFor(Walk3State, seconds);
    public void PlayDrinkOnce() => PlayOnce(DrinkState);
    public void PlayBackOutOnce() => PlayOnce(BackOutState);

    // Play a looping/indefinite clip for N seconds, then return to Idle
    public void PlayFor(string stateName, float seconds)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Co_PlayFor(stateName, seconds));
    }

    // Play a one-shot (non-looping) clip and return to Idle at the end
    public void PlayOnce(string stateName)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Co_PlayOnce(stateName));
    }

    IEnumerator Co_PlayFor(string stateName, float seconds)
    {
        anim.CrossFadeInFixedTime(stateName, fade, 0, 0f);
        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            yield return null;
        }
        anim.CrossFadeInFixedTime(IdleState, fade, 0, 0f);
        routine = null;
    }

    IEnumerator Co_PlayOnce(string stateName)
    {
        anim.CrossFadeInFixedTime(stateName, fade, 0, 0f);

        // wait until we’re actually in the state
        yield return null;
        while (!IsIn(stateName)) yield return null;

        // wait until nearly finished, then go Idle (works even if the clip loops when you control the cutoff)
        while (IsIn(stateName) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.98f)
            yield return null;

        anim.CrossFadeInFixedTime(IdleState, fade, 0, 0f);
        routine = null;
    }

    bool IsIn(string state) => anim.GetCurrentAnimatorStateInfo(0).IsName(state);
}
