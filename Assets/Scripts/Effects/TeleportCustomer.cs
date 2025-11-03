using UnityEngine;
using System.Collections;

public class TeleportCustomer : PotionEffectCustomer
{
    [SerializeField] private GameObject sapo;
    [SerializeField] private DeliverySpot deliverySpot;

    public override void ActivateEffect()
    {
        sapo = deliverySpot.currentCustomer.gameObject;
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        yield return new WaitForSeconds(2f);
        DeactivateEffect();
        Destroy(sapo);
    }

    public override void DeactivateEffect()
    {
        deliverySpot.SwitchCurrentCostumer();
        deliverySpot.SendNewCostumerIn();
    }
}
