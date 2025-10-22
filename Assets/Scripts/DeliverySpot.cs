using UnityEngine;

public class DeliverySpot : MonoBehaviour
{
    [Header("Current Customer")]
    public Costumer currentCustomer;

    [Header("Snap Settings")]
    public Transform snapPoint;         // Empty GameObject on the table where potion should snap
    public float snapSpeed = 10f;       // How fast the potion snaps (higher = faster)
    public bool snapInstantly = true;   // If true, it teleports instead of lerping

    public PotionEffect currentPotion = null;

    private void OnTriggerEnter(Collider other) //Called when the potion is placed
    {
        // Only allow one potion at a time
        if (currentPotion != null) return;
        Debug.Log("hello1");

        PotionEffect potionEffect = other.GetComponent<PotionEffect>();

        if (potionEffect != null && !potionEffect.hasBeenUsed)
        {
            // Lock this potion as the current one
            currentPotion = potionEffect;
            Debug.Log("hello2");

            // Snap potion in place, chosen in inspector
            if (snapInstantly)
            {
                SnapInstantly(other.transform);
            }
            else
            {
                StartCoroutine(SnapSmoothly(other.transform));
            }
            // Run the check
            bool correctPotion = currentCustomer.CheckPotion(potionEffect);

            if (correctPotion == true)
            {
                Debug.Log("hello3");
                potionEffect.hasBeenUsed = true;// Mark as used
                Destroy(other.GetComponent<Rigidbody>());// Disable Rigidbody so it doesn't move after snapping
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the potion leaves the spot, allow a new one to be placed again
        if (currentPotion != null && other.GetComponent<PotionEffect>() == currentPotion)
        {
            currentPotion = null;
        }
    }

    private void SnapInstantly(Transform potion)
    {
        potion.position = snapPoint.position;
        potion.rotation = snapPoint.rotation;
    }

    private System.Collections.IEnumerator SnapSmoothly(Transform potion)
    {
        float t = 0f;
        Vector3 startPos = potion.position;
        Quaternion startRot = potion.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * snapSpeed;
            potion.position = Vector3.Lerp(startPos, snapPoint.position, t);
            potion.rotation = Quaternion.Slerp(startRot, snapPoint.rotation, t);
            yield return null;
        }

        potion.position = snapPoint.position;
        potion.rotation = snapPoint.rotation;
    }

}
