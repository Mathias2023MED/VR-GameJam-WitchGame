using UnityEngine;

public class BreakableBottle : MonoBehaviour
{
    public GameObject brokenVersion; // Prefab med smadret flaske
    public float breakForce = 5f;    // Hvor hårdt man skal ramme for at smadre

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakForce)
        {
            // Spawner den smadrede version
            Instantiate(brokenVersion, transform.position, transform.rotation);
            Destroy(gameObject); // Fjerner den intakte flaske
        }
    }
}
