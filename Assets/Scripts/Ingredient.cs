using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour
{
    public IngredientSO ingredientSO; // Drag the ScriptableObject here in Inspector
    public GameObject prefab;        // Prefab to spawn

    private bool hasBeenGrabbed = false;
    private Vector3 spawnPosition;   // Store original position
    private Quaternion spawnRotation; // Store original rotation

    private void Start()
    {
        // Save the original position of this GameObject
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    // Call this when the ingredient is grabbed or used
    public void SpawnNewIngredient()
    {
        if (hasBeenGrabbed) return; // Only spawn once
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not set on Ingredient!");
            return;
        }
        float delay = 1f;
        StartCoroutine(SpawnAfterDelay(delay)); // Wait 1 second before spawning
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Spawn a new copy at the saved original position
        Instantiate(prefab, spawnPosition, spawnRotation);
        hasBeenGrabbed = true; // Mark this ingredient as already grabbed
    }
}
