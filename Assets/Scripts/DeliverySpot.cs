using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections; // Needed for IEnumerator

public class DeliverySpot : MonoBehaviour
{
    [Header("Current Customer")]
    public Costumer currentCustomer; // Reference to the customer who will receive the potion

    [Header("Snap Settings")]
    public Transform snapPoint;         // Empty GameObject on the table where potion should snap
    public float snapSpeed = 10f;       // How fast the potion snaps (higher = faster)
    public bool snapInstantly = true;   // If true, it teleports instead of lerping

    public PotionEffect currentPotion = null; // Current potion on the delivery spot

    private void OnTriggerEnter(Collider other) // Called when a potion enters the delivery spot collider
    {
        // Only allow one potion at a time
        if (currentPotion != null) return; // Exit if a potion is already on the spot

        PotionEffect potionEffect = other.GetComponent<PotionEffect>(); // Try to get PotionEffect component
        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>(); // Try to get XRGrabInteractable component

        // Proceed only if it's a valid potion and has a grab component
        if (potionEffect != null && grab != null && !potionEffect.hasBeenUsed)
        {
            currentPotion = potionEffect; // Lock this potion as the current one
            StartCoroutine(SnapAfterRelease(grab, potionEffect)); // Start coroutine to snap after release
        }
    }

    private IEnumerator SnapAfterRelease(XRGrabInteractable grab, PotionEffect potionEffect)
    {
        // Wait until the potion is no longer being grabbed
        while (grab.isSelected) // Loop while the potion is still held
        {
            yield return null; // Wait for next frame
        }

        // Snap potion in place
        if (snapInstantly)
        {
            SnapInstantly(grab.transform); // Immediately move potion to snap point
        }
        else
        {
            StartCoroutine(SnapSmoothly(grab.transform)); // Smoothly move potion to snap point
        }

        // Run the check for the current customer
        bool correctPotion = currentCustomer.CheckPotion(potionEffect); // Call CheckPotion on customer

        if (correctPotion) // If the potion is correct
        {
            potionEffect.hasBeenUsed = true; // Mark as used
            Rigidbody rb = grab.GetComponent<Rigidbody>(); // Get Rigidbody component
            if (rb != null) rb.isKinematic = true; // Make Rigidbody kinematic so it stays in place
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the potion leaves the spot, allow a new one to be placed again
        if (currentPotion != null && other.GetComponent<PotionEffect>() == currentPotion)
        {
            currentPotion = null; // Unlock spot
        }
    }

    private void SnapInstantly(Transform potion)
    {
        potion.position = snapPoint.position; // Set potion position to snap point
        potion.rotation = snapPoint.rotation; // Set potion rotation to snap point
    }

    private IEnumerator SnapSmoothly(Transform potion)
    {
        float t = 0f; // Interpolation factor
        Vector3 startPos = potion.position; // Start position of potion
        Quaternion startRot = potion.rotation; // Start rotation of potion

        while (t < 1f) // Loop until interpolation completes
        {
            t += Time.deltaTime * snapSpeed; // Increase interpolation factor over time
            potion.position = Vector3.Lerp(startPos, snapPoint.position, t); // Interpolate position
            potion.rotation = Quaternion.Slerp(startRot, snapPoint.rotation, t); // Interpolate rotation
            yield return null; // Wait for next frame
        }

        potion.position = snapPoint.position; // Ensure final position
        potion.rotation = snapPoint.rotation; // Ensure final rotation
    }

}
