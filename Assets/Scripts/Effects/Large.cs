using UnityEngine;
using System.Collections;

public class Large : MonoBehaviour
{
    public GameObject hand;
    private Vector3 originalScale;
    public Vector3 enlargedScale = new Vector3(2f, 2f, 2f);
    private float speed = 2f;

    void Start()
    {
        originalScale = hand.transform.localScale;
    }

    private void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSizeOverTime(enlargedScale));
    }

    private void Deactivate()
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

