using UnityEngine;
using System.Collections;

public class Costumer : MonoBehaviour
{
    [Header("Requested Potion")]
    public PotionRecipeSO requestedPotion;
    public SapoAnimations sapoAnimations;
    public DeliverySpot deliverySpot;

    public bool CheckPotion(PotionEffect deliveredPotion)
    {
        if (deliveredPotion.potion == requestedPotion)
        {
            Debug.Log("Correct potion delivered!");
            return true;
            // Add drink animation
        }
        else
        {
            Debug.Log("Wrong potion delivered!");
            float delay = 0.5f;
            StartCoroutine(PlayNoAnimationAfterDelay(delay));
            //Make no sound
            return false;
            // Add "no" animation
        }
    }

    private IEnumerator PlayNoAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sapoAnimations.PlayDropKick(); // Call your "no" animation here
    }

    private void Start()
    {
        // Check if this customer is the current one for its delivery spot
        if (deliverySpot != null && deliverySpot.currentCustomer == this)
        {
            // Start walk animation
            Debug.Log("This is the costumer");
            float walkDistance = 4f;
            sapoAnimations.Walk1_Distance(walkDistance);
            StartCoroutine(delay());
        }
        else Debug.Log("No");
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(5f);
        sapoAnimations.PlayWalkingOut(); // Call your "no" animation here
    }

}
