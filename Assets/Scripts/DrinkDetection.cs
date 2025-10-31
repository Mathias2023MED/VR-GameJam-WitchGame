using UnityEngine;

public class DrinkDetection : MonoBehaviour
{
    public string basePotionName = "BasePotion";
    public string emptyBottleTag = "EmptyBottle";

    private void OnTriggerEnter(Collider other)
    {
        PotionEffectWitch potionEffectWitch = other.GetComponent<PotionEffectWitch>();

        if (potionEffectWitch != null && !potionEffectWitch.hasBeenUsed)
        {
            // Trigger the potion effect
            potionEffectWitch.ActivateEffect();
            potionEffectWitch.hasBeenUsed = true;
            // Blend the potion bottle's color back to base
            ColorChanger colorChanger = other.GetComponent<ColorChanger>();
            if (colorChanger != null)
            {
                colorChanger.ChangeColor(basePotionName);
                Debug.Log("Potion effect activated and color changed to base.");
            }
            other.gameObject.tag = emptyBottleTag;
        }
        else
        {
            Debug.Log("No potion effect found on the collided object.");
        }
    }
}
