using UnityEngine;

public class Test : PotionEffect
{
    public override void ActivateEffect()
    {
        Debug.Log("Test Potion Effect Activated!");
    }

    public override void DeactivateEffect()
    {
        Debug.Log("Test Potion Effect Deactivated!");
    }
}
