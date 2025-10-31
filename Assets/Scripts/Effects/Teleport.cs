using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;


public class Teleport : PotionEffect
{
    private Assigner assigner;
    private GameObject player;
    private TeleportationProvider teleportationProvider;
    private Transform spawnPoint;
    private GameObject backrooms;

    private void Start()
    {
        // Find the Assigner in the scene(make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            // Assign all references from the manager
            player = assigner.player;
            teleportationProvider = assigner.teleportationProvider;
            spawnPoint = assigner.spawnPoint;
            backrooms = assigner.backrooms;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
    }

    public override void ActivateEffect()
    {
        backrooms.SetActive(true);
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
        teleportationProvider.enabled = true;
    }

    public override void DeactivateEffect()
    {
        backrooms.SetActive(false);
        teleportationProvider.enabled = false;
    }

}
