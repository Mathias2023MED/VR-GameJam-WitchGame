using UnityEngine;
using System.Collections;

public class Love : MonoBehaviour
{
    public GameObject violence;

    private float waitSeconds = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Activate()
    {
        violence.SetActive(true);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitSeconds);
        violence.SetActive(false);
    }
}
