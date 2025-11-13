using System;
using UnityEngine;

public class SapoBackrooms : MonoBehaviour
{

    public SapoAnimations sapoAnimations;
    private bool hasStarted = false;


    public void StartBackroomsSequence()
    {
        if (hasStarted) return; // prevent double-trigger
        hasStarted = true;

        // Terrified -> DropKick -> Run x10 -> Destroy
        sapoAnimations.PlaySneaking(() =>
        {
            sapoAnimations.PlayDropKick(() =>
            {
                StartRunLoop(20);
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
