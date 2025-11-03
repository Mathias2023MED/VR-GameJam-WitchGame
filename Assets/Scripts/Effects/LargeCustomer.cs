using System.Collections;
using UnityEngine;

public class LargeCustomer : PotionEffectCustomer
{
    public GameObject eye;
    public Assigner assigner;

    public override void ActivateEffect()
    {
        if (eye != null)
            StartCoroutine(ScaleAndMoveEyeCoroutine());
    }

    private void Start()
    {
        // Find the Assigner in the scene (make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            eye = assigner.eye;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
        ActivateEffect();
    }

    private IEnumerator ScaleAndMoveEyeCoroutine()
    {
        // Wait for 2 seconds before starting
        float delay = 10f;
        yield return new WaitForSeconds(delay);

        Vector3 startScale = eye.transform.localScale;
        Vector3 targetScale = new Vector3(1.7f, 1.7f, 1.7f);

        Vector3 startPosition = eye.transform.localPosition;
        Vector3 targetPosition = new Vector3(-0.027f, -0.959f, 0.277f);

        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // linear interpolation factor

            // Lerp scale and position simultaneously
            eye.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            eye.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        // Ensure final scale and position are exact
        eye.transform.localScale = targetScale;
        eye.transform.localPosition = targetPosition;
    }

    public override void DeactivateEffect()
    {
        
    }
}
