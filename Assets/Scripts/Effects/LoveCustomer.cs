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
        float delay = 2f;
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

        deliverySpot.currentCustomer.PlayEffectSound();

        ColorChangerEffect colorChanger = deliverySpot.currentCustomer.colorChangerEffect;
        if (colorChanger != null)
        {
            yield return new WaitForSeconds(delay);
            colorChanger.ChangeColor();
        }
        else
        {
            Debug.LogWarning("ColorChangerEffect not found on current customer!");
        }

        // Wait before triggering the animation
        float delay2 = 4f;
        yield return new WaitForSeconds(delay2);

        // Play the kick animation
        
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
