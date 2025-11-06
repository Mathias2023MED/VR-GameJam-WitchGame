using UnityEngine;

public class EmptyBottle : MonoBehaviour
{
    public GameObject prefab;

    private bool hasBeenGrabbed = false;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private BottleSpawner spawner;

    private void Start()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        // find the spawner (or you could assign it in Inspector)
        spawner = FindFirstObjectByType<BottleSpawner>();
    }

    public void SpawnNewBottle()
    {
        if (hasBeenGrabbed) return;
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not set on Ingredient!");
            return;
        }

        hasBeenGrabbed = true; // mark before starting
        float delay = 1f;

        if (spawner != null)
        {
            spawner.SpawnAfterDelay(prefab, spawnPosition, spawnRotation, delay);
        }
        else
        {
            Debug.LogError("No Spawner found in scene!");
        }
    }
}
