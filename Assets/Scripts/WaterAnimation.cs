using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float riseHeight = 5f;
    public float speed = 1f;

    private Vector3 startPoint;
    private Vector3 endPoint;

    public ParticleSystem waterParticles; // Drag your particle system here
    private Coroutine animationCoroutine;

    public Cat catScript;  // Drag your Cat script object here in the Inspector

    void Start()
    {
        startPoint = transform.position;
        endPoint = startPoint + new Vector3(0f, riseHeight, 0f);

        if (waterParticles != null && waterParticles.isPlaying)
            waterParticles.Stop();
    }

    public void WaterRising()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        if (waterParticles != null && !waterParticles.isPlaying)
            waterParticles.Play();

        animationCoroutine = StartCoroutine(MoveWater(endPoint));
    }

    public void WaterLowering()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        if (waterParticles != null && waterParticles.isPlaying)
            waterParticles.Stop();

        animationCoroutine = StartCoroutine(MoveWater(startPoint, true));
    }

    private System.Collections.IEnumerator MoveWater(Vector3 target, bool isLowering = false)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        animationCoroutine = null;

        if (isLowering && catScript != null)
        {
            catScript.Respawn();
        }
    }
}
