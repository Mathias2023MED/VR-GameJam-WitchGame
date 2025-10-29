using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openYRotation = 100f;    // One direction
    public float closedYRotation = 0f;    // Closed rotation
    public float rotationDuration = 1f;   // How long it takes to open/close

    private bool openPositive = true;     // Tracks which direction to open next
    private Coroutine currentCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Costumer"))
        {
            Debug.Log("Hello");
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            // Determine which direction to open
            float targetY = openPositive ? openYRotation : -openYRotation;
            currentCoroutine = StartCoroutine(RotateDoor(targetY));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Costumer"))
        {
            Debug.Log("Goodbye");
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            // Close the door
            currentCoroutine = StartCoroutine(RotateDoor(closedYRotation));

            // Flip the direction for next time
            openPositive = !openPositive;
        }
    }

    private IEnumerator RotateDoor(float targetY)
    {
        float elapsed = 0f;
        Vector3 startRotation = transform.eulerAngles;
        Vector3 targetRotation = new Vector3(startRotation.x, targetY, startRotation.z);

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            float newY = Mathf.LerpAngle(startRotation.y, targetY, t);
            transform.eulerAngles = new Vector3(startRotation.x, newY, startRotation.z);
            yield return null;
        }

        transform.eulerAngles = targetRotation;
    }

}
