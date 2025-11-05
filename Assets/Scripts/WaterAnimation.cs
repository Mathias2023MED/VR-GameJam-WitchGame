using UnityEngine;
using System.Collections;

public class WaterAnimation : MonoBehaviour
{
    [Header("ANIMATION")]
    public float riseHeight = 5f;
    public float speed = 0.7f;

    private Vector3 startPoint;
    private Vector3 endPoint;

    public ParticleSystem waterParticles; // Drag your particle system here
    private Coroutine animationCoroutine;
    private Coroutine bubblingCoroutine;

    public Cat catScript;  // Drag your Cat script object here

    [Header("SOUND")]
    [SerializeField] private AudioClip bubblingStartClip;
    [SerializeField] private AudioClip bubblingLoopClip;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        startPoint = transform.position;
        endPoint = startPoint + new Vector3(0f, riseHeight, 0f);

        if (waterParticles != null && waterParticles.isPlaying)
            waterParticles.Stop();
    }

    public void WaterRising()
    {
        // Stop any existing animation or sound coroutines
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        if (bubblingCoroutine != null)
            StopCoroutine(bubblingCoroutine);

        // Start particles
        if (waterParticles != null && !waterParticles.isPlaying)
            waterParticles.Play();

        // Animate water rising
        animationCoroutine = StartCoroutine(MoveWater(endPoint));

        // Start bubbling sound sequence
        bubblingCoroutine = StartCoroutine(PlayBubblingSequence());
    }

    public void WaterLowering()
    {
        // Stop any animation or sound coroutines
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        if (bubblingCoroutine != null)
            StopCoroutine(bubblingCoroutine);

        // Stop particles
        if (waterParticles != null && waterParticles.isPlaying)
            waterParticles.Stop();

        // Animate water lowering
        animationCoroutine = StartCoroutine(MoveWater(startPoint, true));

        // Stop any playing audio immediately
        SoundManager.Instance.StopSound(audioSource);
    }

    private IEnumerator MoveWater(Vector3 target, bool isLowering = false)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        animationCoroutine = null;

        // Optional: respawn cat after lowering water
        if (isLowering && catScript != null)
        {
            float delay = 1f;
            yield return new WaitForSeconds(delay);
            catScript.Respawn();
        }
    }

    private IEnumerator PlayBubblingSequence()
    {
        // Play start clip first via SoundManager
        if (bubblingStartClip != null && audioSource != null)
            SoundManager.Instance.PlaySound(audioSource, bubblingStartClip);

        // Wait for the start clip to finish
        if (bubblingStartClip != null)
            yield return new WaitForSeconds(bubblingStartClip.length);

        // Play the looping clip
        if (bubblingLoopClip != null && audioSource != null)
        {
            audioSource.clip = bubblingLoopClip;
            audioSource.loop = true;
            audioSource.Play(); // Looping clip cannot use PlayOneShot
        }
    }
}
