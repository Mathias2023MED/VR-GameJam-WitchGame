using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionRecipe", menuName = "Potion/Recipe")]
public class PotionRecipe : ScriptableObject
{
    public string potionName;
    public GameObject potionPrefab;
    public Ingredient[] ingredients;
}
