using UnityEngine;

public class ToadAnimatorLogic : MonoBehaviour
{
    Animator animator;
    float randomTime;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Choose which walk scenario manually for now
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartWalkScenario(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartWalkScenario(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            StartWalkScenario(3);
    }

    void StartWalkScenario(int scenario)
    {
        animator.SetInteger("WalkScenario", scenario);
        randomTime = Random.Range(2f, 5f);
        StartCoroutine(HandleWalkSequence(scenario));
    }

    System.Collections.IEnumerator HandleWalkSequence(int scenario)
    {
        // Wait for a random time before walking out
        yield return new WaitForSeconds(randomTime);

        if (scenario == 1 || scenario == 2)
            animator.SetTrigger("WalkOutTrigger");

        // Reset to Idle
        yield return new WaitForSeconds(2f);
        animator.SetInteger("WalkScenario", 0);
    }
}
