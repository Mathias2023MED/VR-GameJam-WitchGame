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

    public ColorChangerEffect colorChangerEffect;

    [Header("SPEECH BUBBLE")]
    public GameObject speechBubble;
    public GameObject speechBubbleTeleport;
    public GameObject speechBubbleLOVE;
    public GameObject speechBubbleEnlargement;

    public bool isFirstCustomer = false;

    [Header("SOUND")]
    [SerializeField] private AudioClip noClip;
    [SerializeField] private AudioClip yesClip;
    [SerializeField] private AudioClip drinkingClip;
    [SerializeField] private AudioClip angryClip;
    [SerializeField] private AudioClip orderingClip;
    [SerializeField] private AudioSource audioSource;

    [Header("COLLIDER")]
    [SerializeField] private Collider colliderToToggle; // The collider you want to disable/enable


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

    public void PlayYesSoundAndShakeHead()
    {
        sapoAnimations.PlayNoddingHeadCoroutine();
        //PlayShakeHeadYes
        SoundManager.Instance.PlaySound(audioSource, yesClip);
    }



    public void DrinkPotion(PotionEffectCustomer currentPotion)
    {
        StartCoroutine(DrinkPotionRoutine(currentPotion));
    }
    private IEnumerator DrinkPotionRoutine(PotionEffectCustomer currentPotion)
    {
        DisableSpeechBubble(); // Disables the speech bubble when the correct one is delivered

        float delay = 3f; // or whatever delay you want
        yield return new WaitForSeconds(delay);

        AttachPotionToHand();
        sapoAnimations.PlayDrink(() => currentPotion.ActivateEffect());
        float delay2 = 2f; // or whatever delay you want
        yield return new WaitForSeconds(delay2);
        PlayDrinkingSound();
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
        if (isFirstCustomer)
        {
            Debug.Log("Hello");
            WalkIn();
        }
    }

    public void WalkIn() // Walks the customer into the shop
    {
        if (deliverySpot != null && deliverySpot.currentCustomer == this)
        {
            float delay = 5f;
            StartCoroutine(WalkInRoutine(delay));
        }
    }

    private IEnumerator WalkInRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        float walkDistance = 4f;
        sapoAnimations.Walk1_Distance(walkDistance, false, EnableSpeechBubble);
    }

    private void EnableSpeechBubble() //Enables the correct speechbubble text
    {
        speechBubble.SetActive(true);
        PlayOrderingSound();

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

    public void PlayAngrySound()
    {
        SoundManager.Instance.PlaySound(audioSource, angryClip);
    }

    public void PlayDrinkingSound()
    {
        SoundManager.Instance.PlaySound(audioSource, drinkingClip);
    }

    public void PlayOrderingSound()
    {
        SoundManager.Instance.PlaySound(audioSource, orderingClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Remove") && colliderToToggle != null)
        {
            colliderToToggle.enabled = false;
            Debug.Log("Sapo collider disabled!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Remove") && colliderToToggle != null)
        {
            colliderToToggle.enabled = true;
            Debug.Log("Sapo collider re-enabled!");
        }
    }

}
