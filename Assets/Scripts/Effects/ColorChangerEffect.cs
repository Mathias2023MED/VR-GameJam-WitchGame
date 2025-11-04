using UnityEngine;
using System.Collections;

public class ColorChangerEffect : MonoBehaviour
{
    [Header("Materiale til farveskift")]
    public Material targetMaterial;
    public Color endColor = Color.red;
    public float duration = 2f;

    private Color startColor;
    private Renderer objRenderer;
    private int materialIndex = -1;

    private void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogError("Renderer not found");
            return;
        }

        if (targetMaterial == null)
        {
            Debug.LogError("No material chosen");
            return;
        }

        Material[] mats = objRenderer.materials;
        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i].name.Contains(targetMaterial.name))
            {
                materialIndex = i;
                mats[i] = new Material(mats[i]); // Make instance
                startColor = mats[i].color;
                objRenderer.materials = mats;
                break;
            }
        }

        if (materialIndex == -1)
        {
            Debug.LogWarning("Material not found");
        }
    }

    public void ChangeColor()
    {
        if (materialIndex == -1)
        {
            Debug.LogWarning("Material not set up properly");
            return;
        }

        StopAllCoroutines(); // Stop any previous color changes
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        float timer = 0f;
        Material[] mats = objRenderer.materials;
        Color start = mats[materialIndex].color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            mats[materialIndex].color = Color.Lerp(start, endColor, t);
            yield return null;
        }

        // Ensure final color is exact
        mats[materialIndex].color = endColor;
    }
}
