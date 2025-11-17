using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class CrystalBall : MonoBehaviour
{
    public VideoPlayer idleVideo;
    public VideoPlayer eventVideo;

    public MeshRenderer idleRenderer;
    public MeshRenderer eventRenderer;

    public Collider triggerCollider;

    public float minInterval = 10f;
    public float maxInterval = 30f;

    private bool isEventActive = false;
    private bool isWaiting = false;

    private void Awake()
    {
        triggerCollider.isTrigger = true;

        // Start with idle
        idleRenderer.enabled = true;
        idleVideo.Play();

        eventRenderer.enabled = false;
        eventVideo.enabled = false;
        
    }

    private void Start()
    {
        eventRenderer.enabled = false;
        eventVideo.enabled = false;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        float holdUp = 60f;
        Debug.Log("is waiting");
        yield return new WaitForSeconds(holdUp);
        StartCoroutine(RandomEventRoutine());
    }

    private IEnumerator RandomEventRoutine()
    {
        Debug.Log("routine started");
        while (true)
        {
            if (!isEventActive && !isWaiting)
            {
                isWaiting = true;
                float wait = Random.Range(minInterval, maxInterval);
                yield return new WaitForSeconds(wait);
                isWaiting = false;

                if (!isEventActive)
                    StartEventVideo();
            }
            yield return null;
        }
    }

    private void StartEventVideo()
    {
        Debug.Log("event video started");
        isEventActive = true;

        // Pause idle, hide idle renderer
        idleVideo.Pause();
        idleRenderer.enabled = false;

        // Show event renderer, play event
        eventRenderer.enabled = true;
        eventVideo.enabled = true;
        eventVideo.Play();
    }

    private void StopEventVideo()
    {
        if (!isEventActive) return;
        isEventActive = false;

        // Pause event, hide event renderer
        eventVideo.Stop();
        eventRenderer.enabled = false;

        // Resume idle, show idle renderer
        idleRenderer.enabled = true;
        idleVideo.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && isEventActive)
        {
            StopEventVideo();
        }
    }
}
