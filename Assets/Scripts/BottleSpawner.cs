using UnityEngine;
using System.Collections;

public class BottleSpawner : MonoBehaviour
{
    public void SpawnAfterDelay(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
    {
        StartCoroutine(SpawnCoroutine(prefab, position, rotation, delay));
    }

    private IEnumerator SpawnCoroutine(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(prefab, position, rotation);
    }
}
