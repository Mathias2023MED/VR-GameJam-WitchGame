using System;
using UnityEngine;

public class SapoBackrooms : MonoBehaviour
{
    public SapoAnimations sapoAnimations;

    void Start()
    {
        FightandFligt();
    }

    public void FightandFligt()
    {
        // Terrified -> DropKick -> Run x4 -> Destroy
        sapoAnimations.PlaySneaking(() =>
        {
            sapoAnimations.PlayDropKick(() =>
            {
                StartRunLoop(10);
            });
        });
    }

    void StartRunLoop(int remaining)
    {
        if (remaining <= 0)
        {
            Destroy(gameObject);
            return;
        }

        sapoAnimations.PlayRunning(() =>
        {
            StartRunLoop(remaining - 1);
        });
    }
}
