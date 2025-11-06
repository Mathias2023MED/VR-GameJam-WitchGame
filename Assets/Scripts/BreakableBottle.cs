using UnityEngine;

public class BreakableBottle : MonoBehaviour
{
    public GameObject brokenVersionColored; // Prefab med smadret flaske (med farve)
    public GameObject brokenVersionDefault; // Prefab med smadret flaske (uden farve)
    public float breakForce = 5f;           // Hvor hårdt man skal ramme for at smadre

    [Header("SOUND")]
    [SerializeField] private AudioClip clinkClip;
    [SerializeField] private AudioSource audioSource;
    private float lastClinkTime;
    private float clinkCooldown = 0.2f; // minimum 0.2 sek mellem klir

    private void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude;
        if (impact > breakForce)
        {
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
        else if (impact > 0.2f && Time.time - lastClinkTime > clinkCooldown)
        {
            lastClinkTime = Time.time;
            SoundManager.Instance.PlaySound(audioSource, clinkClip);
        }
    }
}
