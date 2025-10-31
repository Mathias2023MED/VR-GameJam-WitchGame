using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionRecipe", menuName = "Potion/Recipe")]
public class PotionRecipeSO : ScriptableObject
{
    public enum PotionType { love, enlargement, teleportation, failed }
    public PotionType potionType;
    public string potionName;
    public GameObject potionPrefab;
    public IngredientSO[] ingredientsSO;
}
