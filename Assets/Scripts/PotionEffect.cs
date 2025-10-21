using UnityEngine;

public abstract class PotionEffect : MonoBehaviour //Blueprint abstract class for PotionEffects
{
    public float duration = 10f;
    public bool hasBeenUsed = false;
    
    public abstract void ActivateEffect();
    public abstract void DeactivateEffect();
}
