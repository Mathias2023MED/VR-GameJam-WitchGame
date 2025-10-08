using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionRecipe", menuName = "Potion/Recipe")]
public class PotionRecipeSO : ScriptableObject
{
    public string potionName;
    public GameObject potionPrefab;
    public IngredientSO[] ingredientsSO;
}
