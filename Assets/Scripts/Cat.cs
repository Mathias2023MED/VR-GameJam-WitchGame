using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint; // Drag the respawn point GameObject here in the Inspector

    public void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }
        else
        {
            Debug.LogWarning("Respawn point is not assigned on Cat!");
        }

    }
}

