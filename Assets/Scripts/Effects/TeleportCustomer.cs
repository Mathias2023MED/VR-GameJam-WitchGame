using UnityEngine;
using System.Collections;

public class TeleportCustomer : PotionEffectCustomer
{
    [SerializeField] private GameObject sapo;
    [SerializeField] private DeliverySpot deliverySpot;

    [Header("SOUND")]
    [SerializeField] private AudioClip dissapearClip;
    [SerializeField] private AudioSource audioSource;

    public override void ActivateEffect()
    {
        if (deliverySpot == null)
        {
            deliverySpot = FindFirstObjectByType<DeliverySpot>();
        }
        sapo = deliverySpot.currentCustomer.gameObject;
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        PlayDissapearSound();
        yield return new WaitForSeconds(2.6f);
        DeactivateEffect();
        Destroy(sapo);
    }

    public override void DeactivateEffect()
    {
        deliverySpot.SwitchCurrentCostumer();
        deliverySpot.SendNewCostumerIn();
    }

    private void PlayDissapearSound()
    {
        SoundManager.Instance.PlaySound(audioSource, dissapearClip);
    }
}
