using UnityEngine;
using System.Collections;

public class Love : PotionEffect
{
    public GameObject violence;
    public Assigner assigner;

    private float waitSeconds = 20.0f;

    private void Start()
    {
        // Find the Assigner in the scene(make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            violence = assigner.violence;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitSeconds);
        violence.SetActive(false);
    }

    public override void ActivateEffect()
    {
        violence.SetActive(true);
        StartCoroutine(Wait());
    }

    public override void DeactivateEffect()
    {
        //Nothing
    }

}
