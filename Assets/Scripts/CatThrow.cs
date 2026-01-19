using UnityEngine;

public class CatThrow : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;        // Drag for AudioSources 
    public AudioClip[] collisionSounds;    // A List to Add multiple SFX

    [Header("Settings")]
    public float minCollisionVelocity = 0.5f;   // Minimum speed to trigger sound
    public float soundCooldown = 0.3f;          // Prevents spam

    private float lastSoundTime;

    private void Reset()
    {
        // Auto-adds the audioSource if missing
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Not enough time passed? Then ignore
        if (Time.time < lastSoundTime + soundCooldown)
            return;

        // Only play sound if the cat hits something with enough force
        if (collision.relativeVelocity.magnitude < minCollisionVelocity)
            return;

        PlayRandomSound();
        lastSoundTime = Time.time;
    }

    private void PlayRandomSound()
    {
        if (collisionSounds.Length == 0) return;

        int index = Random.Range(0, collisionSounds.Length);
        audioSource.PlayOneShot(collisionSounds[index]);
    }
}
