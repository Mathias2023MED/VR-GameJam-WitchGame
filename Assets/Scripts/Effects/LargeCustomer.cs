using System.Collections;
using UnityEngine;

public class LargeCustomer : PotionEffectCustomer
{
    public GameObject eye;
    public Assigner assigner;
    public override void ActivateEffect()
    {
        if (eye != null)
            StartCoroutine(ScaleEyeCoroutine());
    }

    private void Start()
    {
        // Find the Assigner in the scene(make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            eye = assigner.eye;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
    }

    private IEnumerator ScaleEyeCoroutine()
    {
        // Wait for 2 seconds before starting
        yield return new WaitForSeconds(2f);

        Vector3 startScale = eye.transform.localScale;
        Vector3 targetScale = new Vector3(2f, 2f, 2f);
        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // linear interpolation factor
            eye.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        // Ensure final scale is exactly the target
        eye.transform.localScale = targetScale;
    }

    public override void DeactivateEffect()
    {
        
    }
}
