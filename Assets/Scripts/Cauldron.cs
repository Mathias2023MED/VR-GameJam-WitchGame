using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour
{
    [Header("Recipes")]
    public PotionRecipe[] allRecipes;      
    public Transform spawnPoint;          

    private List<Ingredient> currentIngredients = new List<Ingredient>();

    public void AddIngredient(Ingredient ingredient)
    {
        currentIngredients.Add(ingredient);
    }

    public void BrewPotion()
    {
        foreach (var recipe in allRecipes) // Loop through all available potion recipes
        {
            if (IsMatch(recipe.ingredients.ToList(), currentIngredients)) // Check if the cauldron's current ingredients exactly match this recipe
            {
                Instantiate(recipe.potionPrefab, spawnPoint.position, spawnPoint.rotation);// Spawn potion prefab if match is found
                currentIngredients.Clear(); //Clear the cauldron's ingredient list for the next brew
                return;
            }
        }

        Debug.Log("No matching recipe...");
        currentIngredients.Clear();// Clear the cauldron's ingredient list even if no potion was brewed
        //TODO: TIlføj at en underlig potion bliver tilføjet eller måske en eksplosion i gryden?
    }

    private bool IsMatch(List<Ingredient> recipeIngredients, List<Ingredient> cauldronIngredients) // Helper function to check if the cauldron ingredients match the recipe
    {
        if (recipeIngredients.Count != cauldronIngredients.Count) return false; // If the number of ingredients is different, no match

        foreach (var ing in recipeIngredients) // Loop through each ingredient in the recipe
        {
            if (!cauldronIngredients.Contains(ing)) // If the cauldron does not contain this ingredient, it's not a match
                return false;
        }

        return true; //All ingredients are present in the cauldron, so it's a match
    }
}
