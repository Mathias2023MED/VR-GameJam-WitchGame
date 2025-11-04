using UnityEngine;

public class DrinkDetection : MonoBehaviour
{
    [Header("Base Potion Settings")]
    public string basePotionName = "BasePotion";
    public string emptyBottleTag = "EmptyBottle";

    [Header("Potion Effect References")]
    public PotionEffectWitch loveEffect;
    public PotionEffectWitch teleportEffect;
    public PotionEffectWitch enlargementEffect;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        PotionEffectWitch chosenEffect = null;

        // Find ud af hvilken effekt der skal aktiveres
        switch (tag)
        {
            case "LOVE":
                chosenEffect = loveEffect;
                break;

            case "Teleport":
                chosenEffect = teleportEffect;
                break;

            case "Enlargement":
                chosenEffect = enlargementEffect;
                break;

            default:
                Debug.Log("No recognized potion tag found on the collided object.");
                break;
        }

        // Hvis vi fandt en gyldig effekt, aktiver den
        if (chosenEffect != null && !chosenEffect.hasBeenUsed)
        {
            chosenEffect.ActivateEffect();
            chosenEffect.hasBeenUsed = true;

            // Skift farve på flasken (valgfrit)
            ColorChanger colorChanger = other.GetComponent<ColorChanger>();
            if (colorChanger != null)
            {
                colorChanger.ChangeColor(basePotionName);
                Debug.Log($"Potion '{tag}' activated and bottle color reset.");
            }

            // Skift tag til 'EmptyBottle'
            other.gameObject.tag = emptyBottleTag;
        }
    }
}
