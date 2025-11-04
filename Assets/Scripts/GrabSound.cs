using UnityEngine;

public class GrabSound : MonoBehaviour
{
    [Header("SOUND")]
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioSource audioSource;

    public void PlayGrabSound()
    {
        SoundManager.Instance.PlaySound(audioSource, grabClip);
    }
}
