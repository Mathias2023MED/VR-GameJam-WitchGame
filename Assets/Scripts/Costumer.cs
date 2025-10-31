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

    [Header("REQUESTED POTION")]
    public PotionRecipeSO requestedPotion;
    public SapoAnimations sapoAnimations;
    public DeliverySpot deliverySpot;

    [Header("DRINK ANIMATION")]
    [SerializeField] private Transform attachPoint;

    [Header("SPEECH BUBBLE")]
    public GameObject speechBubble;
    public GameObject speechBubbleTeleport;
    public GameObject speechBubbleLOVE;
    public GameObject speechBubbleEnlargement;

    [Header("SOUND")]
    [SerializeField] private AudioClip noClip;
    [SerializeField] private AudioSource audioSource;


    public bool CheckPotion(PotionEffectCustomer deliveredPotion) //helper function
    {
        if (deliveredPotion.potion == requestedPotion) //Correct potion delivered
        {
            Debug.Log("Correct potion delivered!");
            return true;
        }
        else //Wrong potion delivered
        {
            Debug.Log("Wrong potion delivered!");
            return false;
        }
    }

    public void ShakeHead()
    {
        sapoAnimations.PlayShakingHeadCoroutine();
        SoundManager.Instance.PlaySound(audioSource, noClip);
    }

    public void DrinkPotion(PotionEffectCustomer currentPotion)
    {
        DisableSpeechBubble(); //Disables the speech bubble when the correct one is delivered
        AttachPotionToHand(); //todo: NOT WORKING
        //TODO: Make a small coroutine that adds a second delay
        sapoAnimations.PlayDrink(() => currentPotion.ActivateEffect());
    }

    private void AttachPotionToHand()
    {
        deliverySpot.placedPotion.transform.SetParent(attachPoint, false); // false means keep localPosition as is
        deliverySpot.placedPotion.transform.localPosition = Vector3.zero;
        deliverySpot.placedPotion.transform.localRotation = Quaternion.identity;
    }

    private void Start()
    {
        DisableSpeechBubble();
        WalkIn();
    }

    private void WalkIn() //Walks the costumer into the shop
    {
        // Check if this customer is the current one for its delivery spot
        if (deliverySpot != null && deliverySpot.currentCustomer == this)
        {
            // Start walk animation
            float walkDistance = 4f;
            sapoAnimations.Walk1_Distance(walkDistance, false, EnableSpeechBubble);
        }
        else
            return;
    }

    private void EnableSpeechBubble() //Enables the correct speechbubble text
    {
        speechBubble.SetActive(true);
        //todo: Play a speaking sound?

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
                Debug.LogWarning("Nothing fits");
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
