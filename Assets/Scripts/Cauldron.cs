using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour
{
    public WaterAnimation waterAnimation;
    public ColorChanger colorChangerWater;
    //public ColorChanger colorChangerBubbles;

    public string failedPotion = "FailedPotion";
    public string basePotionName = "BasePotion";
    public PotionRecipeSO failedPotionRecipe; // Drag your “Failed Potion” SO here

    public Transform tempSpawnPoint;

    [Header("Recipes")]
    public PotionRecipeSO[] allRecipes;      
    public PotionRecipeSO brewedPotion;
    public bool canAddIngredient = false;
    public bool waterInCauldron = false;


    public List<IngredientSO> currentIngredients = new List<IngredientSO>();

    public void AddIngredient(IngredientSO ingredientSO)
    {
        currentIngredients.Add(ingredientSO);
        Debug.Log("Ingredient added");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has an IngredientObject script
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null & canAddIngredient)
        {
            // Add the ingredient to the Cauldron
            AddIngredient(ingredient.ingredientSO);

            // Optionally destroy the ingredient object (simulate it dissolving)
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Spoon") && canAddIngredient)
        {
            // Call your mix function, or trigger some effect
            BrewPotion();
            Debug.Log("Spoon used to mix potion!");
        }
        else if (other.CompareTag("Spoon"))
        {
            return;
        }
        if (other.CompareTag("EmptyBottle"))
        {
            // Call your mix function, or trigger some effect
            FillBottle(other);
            Debug.Log("Bottled Filled");
        }
        else if (other.CompareTag("Wand"))
        {
            if (waterAnimation != null)
            {
                waterAnimation.WaterRising(); // Trigger the rising animation
                waterInCauldron = true;
                canAddIngredient = true;
                Debug.Log("Water is rising!");
            }
        }
        else if (other.CompareTag("Cat"))
        {
            if (waterAnimation != null && waterInCauldron)
            {
                colorChangerWater.ChangeColor(basePotionName);
                waterAnimation.WaterLowering(); // Trigger the rising animation
                currentIngredients.Clear();
                canAddIngredient = false;
                Debug.Log("Water is lowering!");
                waterInCauldron = false;
            }
        }
        else if (other.CompareTag("Cat"))
        {
            return;
        }
        else
        {
            Destroy(other.gameObject);
        }

    }

    public void BrewPotion()
    {
        foreach (var recipe in allRecipes) // Loop through all available potion recipes
        {
            if (IsMatch(recipe.ingredientsSO.ToList(), currentIngredients)) // Check if the cauldron's current ingredients exactly match this recipe
            {
                brewedPotion = recipe; //Save it as the brewed result
                currentIngredients.Clear();
                colorChangerWater.ChangeColor(recipe.potionName);
                //colorChangerBubbles.ChangeColor(recipe.potionName);
                return;
            }
        }
        brewedPotion = failedPotionRecipe; //Failed potion, if nothing fits.
        currentIngredients.Clear();// Clear the cauldron's ingredient list even if no potion was brewed
        colorChangerWater.ChangeColor(failedPotionRecipe.name);
        canAddIngredient = false;
    }

    public void FillBottle(Collider emptyBottle)
    {
        if (brewedPotion == null || brewedPotion.potionPrefab == null)
        {
            Debug.LogWarning("No potion to fill!");
            return;
        }

        // Store the bottle's position and rotation
        Vector3 bottlePos = emptyBottle.transform.position;
        Quaternion bottleRot = emptyBottle.transform.rotation;

        // Destroy the empty bottle
        Destroy(emptyBottle.gameObject);

        // Spawn the brewed potion prefab at the same position
        //GameObject spawnedPotion = Instantiate(brewedPotion.potionPrefab, bottlePos, bottleRot);
        GameObject spawnedPotion = Instantiate(brewedPotion.potionPrefab, tempSpawnPoint.position, tempSpawnPoint.rotation);

        // Optional: Parent it to the player's hand if using XR Toolkit
        // spawnedPotion.transform.SetParent(playerHandTransform, true);
    }

    public void ResetCauldron()
    {
        currentIngredients.Clear();
        brewedPotion = null;
    }

    private bool IsMatch(List<IngredientSO> recipeIngredientsSO, List<IngredientSO> cauldronIngredients) // Helper function to check if the cauldron ingredients match the recipe
    {
        if (recipeIngredientsSO.Count != cauldronIngredients.Count) return false; // If the number of ingredients is different, no match

        foreach (var ing in recipeIngredientsSO) // Loop through each ingredient in the recipe
        {
            if (!cauldronIngredients.Contains(ing)) // If the cauldron does not contain this ingredient, it's not a match
                return false;
        }

        return true; //All ingredients are present in the cauldron, so it's a match
    }
}
