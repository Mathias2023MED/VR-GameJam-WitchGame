using UnityEngine;

public class Wand : MonoBehaviour
{
    [Header("SOUND")]
    [SerializeField] private AudioClip wandClip;
    [SerializeField] private AudioSource audioSource;

    public void PlayWandSound()
    {
        SoundManager.Instance.PlaySound(audioSource, wandClip);
    }


}
