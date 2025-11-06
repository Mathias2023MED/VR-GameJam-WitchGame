using UnityEngine;

public class EmptyBottle : MonoBehaviour
{

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
        hasBeenGrabbed = true; // mark before starting
        float delay = 1f;

        if (spawner != null)
        {
            spawner.SpawnAfterDelay(spawnPosition, spawnRotation, delay);
        }
        else
        {
            Debug.LogError("No Spawner found in scene!");
        }
    }
}
