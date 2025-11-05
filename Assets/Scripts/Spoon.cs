using UnityEngine;

public class Spoon : MonoBehaviour
{
    [Header("SOUND")]
    [SerializeField] private AudioClip spoonClip;
    [SerializeField] private AudioSource audioSource;

    public void PlaySpoonSound()
    {
        SoundManager.Instance.PlaySound(audioSource, spoonClip);
    }
}
