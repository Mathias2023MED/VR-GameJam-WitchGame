using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;


public class Teleport : PotionEffectWitch
{
    private Assigner assigner;
    private GameObject player;
    private GameObject teleportationInteractor;
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
            teleportationInteractor = assigner.teleportationInteractor;
            spawnPoint = assigner.spawnPoint;
            backrooms = assigner.backrooms;

            teleportationInteractor.SetActive(false);
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
        teleportationInteractor.SetActive(true);
    }

    public override void DeactivateEffect()
    {
        backrooms.SetActive(false);
        teleportationInteractor.SetActive(false);
    }

}
