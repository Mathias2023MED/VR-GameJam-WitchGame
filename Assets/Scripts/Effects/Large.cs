using UnityEngine;
using System.Collections;

public class Large : PotionEffectWitch
{
    private Assigner assigner;
    public GameObject hand;
    private Vector3 originalScale;
    public Vector3 enlargedScale = new Vector3(2f, 2f, 2f);
    public float speed = 1f;

    void Start()
    {
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
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
        Debug.Log("BIG HAND");
        StartCoroutine(Activate());
        DeactivateEffect();
    }

    public override void DeactivateEffect()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Activate()
    {
        // Grow the hand
        float delay = 2f;
        yield return new WaitForSeconds(delay);
        StartCoroutine(ChangeSizeOverTime(enlargedScale));
    }

    private IEnumerator Deactivate()
    {
        // Stay large for duration seconds
        yield return new WaitForSeconds(duration);

        // Shrink back
        yield return StartCoroutine(ChangeSizeOverTime(originalScale));
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

