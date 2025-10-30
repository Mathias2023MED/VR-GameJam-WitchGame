using UnityEngine;
using System.Collections;

public class BrokenGlass : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(DestroyGlass());
    }

    private IEnumerator DestroyGlass()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }


}
