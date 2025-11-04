using UnityEngine;
using System.Collections;

public class LoveCustomer : PotionEffectCustomer
{
    private DeliverySpot deliverySpot;
    private SapoAnimations sapoAnimation;

    private void Start()
    {
        ActivateEffect();
    }

    public override void ActivateEffect()
    {
        StartCoroutine(ActivateEffectCoroutine());
    }

    private IEnumerator ActivateEffectCoroutine()
    {
        yield return new WaitForSeconds(10f);
        // Get the ColorChangerEffect on the current customer
        // If deliverySpot is not assigned in Inspector, find it in the scene
        if (deliverySpot == null)
        {
            deliverySpot = FindFirstObjectByType<DeliverySpot>();
        }
        if (sapoAnimation == null)
        {
            sapoAnimation = deliverySpot.currentCustomer.sapoAnimations;
        }

        ColorChangerEffect colorChanger = deliverySpot.currentCustomer.colorChangerEffect;
        if (colorChanger != null)
        {
            colorChanger.ChangeColor();
        }
        else
        {
            Debug.LogWarning("ColorChangerEffect not found on current customer!");
        }

        // Wait before triggering the animation
        float delay = 2f;
        yield return new WaitForSeconds(delay);

        // Play the kick animation
        deliverySpot.currentCustomer.PlayAngrySound();
        sapoAnimation.HurricaneKick_Distance(6f, true);
    }

    public override void DeactivateEffect()
    {
        // Optional cleanup logic if needed later
    }
}
