using System.Collections;
using UnityEngine;

public class LargeCustomer : PotionEffectCustomer
{
    public GameObject head;
    public Assigner assigner;
    public DeliverySpot deliverySpot;
    private SapoAnimations sapoAnimation;

    public override void ActivateEffect()
    {
        // Find the Assigner in the scene (make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            head = assigner.head;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
        if (sapoAnimation == null)
        {
            sapoAnimation = deliverySpot.currentCustomer.sapoAnimations;
        }
        else
        {
            Debug.LogWarning("No SapoAnimations found in the scene!");
        }
        if (head != null)
            StartCoroutine(ScaleAndMoveHeadCoroutine());
    }

    private IEnumerator ScaleAndMoveHeadCoroutine()
    {
        // Wait for 2 seconds before starting
        float delay = 10f;
        yield return new WaitForSeconds(delay);

        Vector3 startScale = head.transform.localScale;
        Vector3 targetScale = new Vector3(1.7f, 1.7f, 1.7f);

        Vector3 startPosition = head.transform.localPosition;
        Vector3 targetPosition = new Vector3(-0.027f, -0.959f, 0.277f);

        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // linear interpolation factor

            // Lerp scale and position simultaneously
            head.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            head.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        // Ensure final scale and position are exact
        head.transform.localScale = targetScale;
        head.transform.localPosition = targetPosition;
        sapoAnimation.WalkingOut_Distance(5f, useRoot: true, preTurnYawDeg: 40f, preTurnTime: 0.15f, restoreRotation: true, restoreTime: 0.15f);
        DeactivateEffect();
    }

    public override void DeactivateEffect()
    {
        StartCoroutine(DeactivateEffectCoroutine());
    }

    private IEnumerator DeactivateEffectCoroutine()
    {
        // Wait 5 seconds before changing customers
        float delay = 5f;
        yield return new WaitForSeconds(delay);

        // Then switch and send in the new customer
        deliverySpot.SwitchCurrentCostumer();
        deliverySpot.SendNewCostumerIn();
    }
}
