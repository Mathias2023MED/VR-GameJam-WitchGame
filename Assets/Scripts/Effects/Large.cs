using UnityEngine;
using System.Collections;

public class Large : PotionEffect
{
    private Assigner assigner;
    public GameObject hand;
    private Vector3 originalScale;
    public Vector3 enlargedScale = new Vector3(2f, 2f, 2f);
    private float speed = 2f;

    void Start()
    {
        // Find the Assigner in the scene(make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            // Assign all references from the manager
            hand = assigner.hand;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
        originalScale = hand.transform.localScale;

    }

    public override void ActivateEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSizeOverTime(enlargedScale));
    }

    public override void DeactivateEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSizeOverTime(originalScale));
    }

    private IEnumerator ChangeSizeOverTime(Vector3 targetScale)
    {
        Vector3 startScale = hand.transform.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            hand.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        hand.transform.localScale = targetScale;
    }
}

