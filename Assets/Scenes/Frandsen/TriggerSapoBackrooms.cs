using UnityEngine;

public class TriggerSapoBackrooms : MonoBehaviour
{
    public SapoBackrooms sapoBackrooms;

    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player"))
         {
              // Prevent further trigger events immediately
              var col = GetComponent<Collider>();
              if (col != null)
              {
                  col.enabled = false;
              }

              // Ensure the backrooms sequence runs
              if (sapoBackrooms != null)
              {
                  sapoBackrooms.StartBackroomsSequence();
              }
              else
              {
                  Debug.LogWarning("TriggerSapoBackrooms: sapoBackrooms reference is null. StartBackroomsSequence not called.");
              }

              // Remove this trigger so it won't run again
              Destroy(gameObject);
                
         }
    }
}
