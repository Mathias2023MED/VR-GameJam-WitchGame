using UnityEngine;
using System.Collections;
using System;

public class Costumer : MonoBehaviour
{
    /* Calling Animation clips:
    sapoAnimations.PlayDrink();
    sapoAnimations.PlayShakingHead();
    sapoAnimations.PlayDropKick(); 
    
    sapoAnimations.Walk1_Distance(2f);           2f = 2 meters     
    sapoAnimations.Walk2_Distance(2f);
    sapoAnimations.Walk3_Distance(2f);
    sapoAnimations.Running_Distance(5f);              

    sapoAnimations.WalkingOut_Distance(5f, useRoot: true, preTurnYawDeg: 40f, preTurnTime: 0.15f, restoreRotation: true, restoreTime: 0.15f);         
    sapoAnimations.HurricaneKick_Distance(6f, true);  
    */

    [Header("Requested Potion")]
    public PotionRecipeSO requestedPotion;
    public SapoAnimations sapoAnimations;
    public DeliverySpot deliverySpot;

    [Header("Drink Animation")]
    [SerializeField] private Transform attachPoint;

    [Header("Speech Bubble")]
    public GameObject speechBubble;
    public GameObject speechBubbleTeleport;
    public GameObject speechBubbleLOVE;
    public GameObject speechBubbleEnlargement;


    public bool CheckPotion(PotionEffect deliveredPotion)
    {
        if (deliveredPotion.potion == requestedPotion)
        {
            Debug.Log("Correct potion delivered!");
            return true;
        }
        else
        {
            Debug.Log("Wrong potion delivered!");
            float delay = 0.5f;
            StartCoroutine(PlayShakingHeadDelay(delay));
            //Make no sound
            return false;
        }
    }

    public void DrinkPotion()
    {
        DisableSpeechBubble(); //Disables the speech bubble when the correct one is delivered
        float delay = 3f;
        AttachPotionToHand();
        StartCoroutine(PlayDrinkDelay(delay));
    }

    private void AttachPotionToHand()
    {
        deliverySpot.placedPotion.transform.SetParent(attachPoint, false); // false means keep localPosition as is
        deliverySpot.placedPotion.transform.localPosition = Vector3.zero;
        deliverySpot.placedPotion.transform.localRotation = Quaternion.identity;
    }

    private IEnumerator PlayShakingHeadDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sapoAnimations.PlayShakingHead(); // Call your "no" animation here
    }

    private IEnumerator PlayDrinkDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sapoAnimations.PlayDrink(); // Call your "no" animation here
    }

    private void Start()
    {
        DisableSpeechBubble();
        WalkIn();
    }

    private void WalkIn()
    {
        // Check if this customer is the current one for its delivery spot
        if (deliverySpot != null && deliverySpot.currentCustomer == this)
            {
                // Start walk animation
                Debug.Log("This is the costumer");
                float walkDistance = 4f;
                sapoAnimations.Walk1_Distance(walkDistance, false, EnableSpeechBubble);
            }
            else Debug.Log("No");
    }

    private void EnableSpeechBubble()
    {
        speechBubble.SetActive(true);

        switch (requestedPotion.potionType)
        {
            case PotionRecipeSO.PotionType.love:
                speechBubbleLOVE.SetActive(true);
                break;
            case PotionRecipeSO.PotionType.enlargement:
                speechBubbleEnlargement.SetActive(true);
                break;
            case PotionRecipeSO.PotionType.teleportation:
                speechBubbleTeleport.SetActive(true);
                break;
            default:
                Debug.LogWarning("Nothing fits"); // ✅ log a warning
                break;
        }
    }
    private void DisableSpeechBubble()
    {
        speechBubble.SetActive(false);
        speechBubbleLOVE.SetActive(false);
        speechBubbleTeleport.SetActive(false);
        speechBubbleEnlargement.SetActive(false);

    }


}
