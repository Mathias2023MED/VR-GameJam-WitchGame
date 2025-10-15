using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float riseHeight = 5f; // How much the water should rise
    public float speed = 1f;       // Speed of water movement

    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool isAnimating = false;
    private bool rising = false;

    void Start()
    {
        // Set start point to current position
        startPoint = transform.position;

        // Set end point by adding riseHeight to Y
        endPoint = startPoint + new Vector3(0f, riseHeight, 0f);
    }

    // Call this to start the water rising animation
    public void WaterRising()
    {
        isAnimating = true;
        rising = true;
    }

    // Call this to start the water lowering animation
    public void WaterLowering()
    {
        isAnimating = true;
        rising = false;
    }

    void Update()
    {
        if (!isAnimating) return;

        // Determine target position
        Vector3 target = rising ? endPoint : startPoint;

        // Move water towards target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Stop animating when we reach the target
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;
            isAnimating = false;
        }
    }
}
