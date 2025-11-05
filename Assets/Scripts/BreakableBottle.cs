using UnityEngine;

public class BreakableBottle : MonoBehaviour
{
    public GameObject brokenVersionColored; // Prefab med smadret flaske (med farve)
    public GameObject brokenVersionDefault; // Prefab med smadret flaske (uden farve)
    public float breakForce = 5f;           // Hvor hårdt man skal ramme for at smadre

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakForce)
        {
            // 🔧 Use CompareTag("TagName"), not CompareTag == "TagName"
            if (CompareTag("EmptyBottle"))
            {
                Instantiate(brokenVersionDefault, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(brokenVersionColored, transform.position, transform.rotation);
            }

            Destroy(gameObject); // Fjern den intakte flaske
        }
    }
}
