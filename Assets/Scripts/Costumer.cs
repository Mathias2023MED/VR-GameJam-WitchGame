using UnityEngine;
using System.Collections;

public class Costumer : MonoBehaviour
{
    [Header("Requested Potion")]
    public PotionRecipeSO requestedPotion;
    public SapoAnimations sapoAnimations;

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


}
