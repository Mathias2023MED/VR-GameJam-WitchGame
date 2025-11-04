using UnityEngine;
using System.Collections;

public class LoveCustomer : PotionEffectCustomer
{
    private DeliverySpot deliverySpot;
    private SapoAnimations sapoAnimation;

    public override void ActivateEffect()
    {
        //todo: make Sapo effects
        StartCoroutine(ActivateEffectCoroutine());
    }

    private IEnumerator ActivateEffectCoroutine()
    {
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
        sapoAnimation.PlayHurricaneKick();
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
