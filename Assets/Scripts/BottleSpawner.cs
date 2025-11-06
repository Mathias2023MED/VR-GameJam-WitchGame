using UnityEngine;
using System.Collections;

public class BottleSpawner : MonoBehaviour
{
    public GameObject prefab;
    public void SpawnAfterDelay(Vector3 position, Quaternion rotation, float delay)
    {
        StartCoroutine(SpawnCoroutine(position, rotation, delay));
    }

    private IEnumerator SpawnCoroutine(Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject newBottle = Instantiate(prefab, position, rotation);
        Rigidbody rb = newBottle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false; // optional, depends on your setup
        }
    }
}
