using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;


public class Teleport : MonoBehaviour
{
    public GameObject player;
    public TeleportationProvider teleportationProvider;
    public Transform spawnPoint;
    public GameObject backrooms;

    private void Activate()
    {
        backrooms.SetActive(true);
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
        teleportationProvider.enabled = true;
    }

    private void Deactivate()
    {
        backrooms.SetActive(false);
        teleportationProvider.enabled = false;
    }

}
