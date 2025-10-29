using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openYRotation = 100f;
    public float closedYRotation = 0f;
    public float rotationDuration = 1f; // Hvor lang tid det tager at åbne/lukke

    private Coroutine currentCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Costumer"))
        {
            Debug.Log("Hello");
            // Stop evt. igangværende rotation og start åbning
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(RotateDoor(openYRotation));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Goodbye");
        if (other.CompareTag("Costumer"))
        {
            // Stop evt. igangværende rotation og start lukning
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(RotateDoor(closedYRotation));
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

        // Sørg for at nå præcis target
        transform.eulerAngles = targetRotation;
    }

}
