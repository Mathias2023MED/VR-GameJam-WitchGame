using UnityEngine;
using System.Collections;

public class Introduction : MonoBehaviour
{
    [SerializeField] private GameObject sapo;
    [SerializeField] private GameObject crystalBall;
    [SerializeField] private AudioClip catClip;
    [SerializeField] private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sapo.SetActive(false);
        crystalBall.SetActive(false);
        StartCoroutine(CatMonolouge());
        
    }

    private IEnumerator CatMonolouge()
    {
        SoundManager.Instance.PlaySound(audioSource, catClip);

        float delay = 40f;
        yield return new WaitForSeconds(delay);

        crystalBall.SetActive(true);

        float delay2 = 5f;
        yield return new WaitForSeconds(delay2);

        sapo.SetActive(true);
    }
}
