using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class Assigner : MonoBehaviour
{
    [Header("LOVE")]
    public GameObject violence;

    [Header("TELEPORT")]
    public GameObject player;
    public TeleportationProvider teleportationProvider;
    public Transform spawnPoint;
    public GameObject backrooms;

    [Header("ENLARGEMENT")]
    public GameObject hand;
}
