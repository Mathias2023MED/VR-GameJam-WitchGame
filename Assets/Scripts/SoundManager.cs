using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioSource audioSource, AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            // Stop whatever is currently playing
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Play the new sound
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Clip or AudioSource missing on " + gameObject.name);
        }
    }


    public void StopSound(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        else
        {
            Debug.LogWarning("AudioSource missing on " + gameObject.name);
        }
    }
}
