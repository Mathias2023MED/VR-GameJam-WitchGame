using UnityEngine;

public class TriggerSapoBackrooms : MonoBehaviour
{
    [SerializeField] private SapoBackrooms targetBackrooms; // reference to the object that animates

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetBackrooms.StartBackroomsSequence();
        }
    }
}
