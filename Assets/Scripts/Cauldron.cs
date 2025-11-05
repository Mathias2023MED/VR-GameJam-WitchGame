using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections; // Needed for IEnumerator
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Cauldron : MonoBehaviour
{
    [Header("References")]
    public WaterAnimation waterAnimation;
    public ColorChanger colorChangerWater;

    [Header("Potion Setup")]
    public string failedPotion = "FailedPotion";
    public string basePotionName = "BasePotion";
    public PotionRecipeSO failedPotionRecipe; // Assign your "Failed Potion" SO here
    public PotionRecipeSO[] allRecipes;       // All valid potion recipes
    public PotionRecipeSO brewedPotion;       // Result of brewing

    [Header("Cauldron State")]
    public bool canAddIngredient = false;
    public bool waterInCauldron = false;

    [Header("Ingredient Tracking")]
    public List<IngredientSO> currentIngredients = new List<IngredientSO>();

    // ==============================
    // INGREDIENT HANDLING
    // ==============================
    public void AddIngredient(IngredientSO ingredientSO)
    {
        currentIngredients.Add(ingredientSO);
        Debug.Log("Ingredient added: " + ingredientSO.name);
    }

    // ==============================
    // COLLISION EVENTS
    // ==============================
    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();

        // Add ingredients
        if (ingredient != null && canAddIngredient)
        {
            AddIngredient(ingredient.ingredientSO);
            Destroy(other.gameObject);
            return;
        }

        // Stir with spoon
        if (other.CompareTag("Spoon"))
        {
            if (canAddIngredient)
            {
                BrewPotion();
                canAddIngredient = false;
                Debug.Log("Spoon used to mix potion!");
            }
            return;
        }

        // Fill bottles
        if (other.CompareTag("EmptyBottle"))
        {
            FillBottle(other);
            return;
        }

        // Add water
        if (other.CompareTag("Wand"))
        {
            if (waterAnimation != null)
            {
                waterAnimation.WaterRising();
                waterInCauldron = true;
                canAddIngredient = true;
                Debug.Log("Water is rising!");
            }
            return;
        }

        // Reset with cat
        if (other.CompareTag("Cat"))
        {
            if (waterAnimation != null && waterInCauldron)
            {
                colorChangerWater.ChangeColor(basePotionName);
                waterAnimation.WaterLowering();
                currentIngredients.Clear();
                canAddIngredient = false;
                waterInCauldron = false;
                Debug.Log("Water is lowering!");
            }
            return;
        }

        // Destroy any other object thrown in
        Destroy(other.gameObject);
    }

    // ==============================
    // BREWING LOGIC
    // ==============================
    public void BrewPotion()
    {
        foreach (var recipe in allRecipes)
        {
            if (IsMatch(recipe.ingredientsSO.ToList(), currentIngredients))
            {
                brewedPotion = recipe;
                currentIngredients.Clear();
                colorChangerWater.ChangeColor(recipe.potionName);
                Debug.Log($"Brewed potion: {recipe.potionName}");
                return;
            }
        }

        // No match → failed potion
        brewedPotion = failedPotionRecipe;
        currentIngredients.Clear();
        colorChangerWater.ChangeColor(failedPotionRecipe.name);
        canAddIngredient = false;
        Debug.Log("Brew failed! Created FailedPotion.");
    }

    // ==============================
    // BOTTLE FILLING LOGIC
    // ==============================
    public void FillBottle(Collider emptyBottle)
    {
        XRGrabInteractable grabInteractable = emptyBottle.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null || grabInteractable.isSelected == false)
        {
            Debug.Log("Bottle not being held — cannot fill.");
            return;
        }

        if (brewedPotion == null || brewedPotion.potionPrefab == null)
        {
            Debug.LogWarning("No brewed potion available to fill!");
            return;
        }

        // Store who is holding the bottle
        IXRSelectInteractor currentInteractor = grabInteractable.firstInteractorSelecting;

        // Save transform info before destroying
        Vector3 bottlePos = emptyBottle.transform.position;
        Quaternion bottleRot = emptyBottle.transform.rotation;

        // Destroy the old empty bottle
        Destroy(emptyBottle.gameObject);

        // Instantiate the filled bottle at the same position
        GameObject newBottle = Instantiate(brewedPotion.potionPrefab, bottlePos, bottleRot);

        // Temporarily disable physics until it’s fully grabbed
        Rigidbody rb = newBottle.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Get its grab component
        XRGrabInteractable newGrab = newBottle.GetComponent<XRGrabInteractable>();

        // Regrab after a short delay to ensure everything’s cleaned up
        if (newGrab != null && currentInteractor != null)
        {
            StartCoroutine(RegrabNextFrame(currentInteractor, newGrab, rb));
        }

        Debug.Log("Bottle filled and will be placed in player's hand.");
    }

    private IEnumerator RegrabNextFrame(IXRSelectInteractor interactor, XRGrabInteractable newGrab, Rigidbody rb)
    {
        // Wait a few frames to ensure Unity has destroyed and updated references
        yield return new WaitForSeconds(0.1f);

        // Find the XRInteractionManager
        XRInteractionManager manager = (interactor as Component)
            ? ((Component)interactor).GetComponentInParent<XRInteractionManager>()
            : null;

        if (manager == null)
            manager = FindFirstObjectByType<XRInteractionManager>();

        if (manager != null)
        {
            manager.SelectEnter(interactor, newGrab);
            Debug.Log("Forced grab on filled bottle.");

            // Wait a moment before restoring physics
            yield return new WaitForSeconds(0.05f);
            if (rb != null) rb.isKinematic = false;
        }
        else
        {
            Debug.LogWarning("No XRInteractionManager found to handle manual grab.");
            if (rb != null) rb.isKinematic = false;
        }
    }

    // ==============================
    // HELPERS
    // ==============================
    public void ResetCauldron()
    {
        currentIngredients.Clear();
        brewedPotion = null;
        Debug.Log("Cauldron reset.");
    }

    private bool IsMatch(List<IngredientSO> recipeIngredientsSO, List<IngredientSO> cauldronIngredients)
    {
        if (recipeIngredientsSO.Count != cauldronIngredients.Count)
            return false;

        foreach (var ing in recipeIngredientsSO)
        {
            if (!cauldronIngredients.Contains(ing))
                return false;
        }
        return true;
    }
}
