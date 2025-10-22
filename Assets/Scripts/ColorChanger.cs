using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class ColorChanger : MonoBehaviour
{
    public Color baseColor;
    public Color redColor;
    public Color greenColor;
    public Color blackColor;
    public Color whiteColor;
    [SerializeField] private float blendDuration = 15f;
    [SerializeField] private Renderer Renderer;

    public void ChangeColor(string potionName)
    {
        switch (potionName)
        {
            case "Level of Violence Elevated":
                StartCoroutine(BlendColor(redColor));
                break;

            case "Random Teleportation":
                StartCoroutine(BlendColor(blackColor));
                break;

            case "Enlargement":
                StartCoroutine(BlendColor(greenColor));
                break;

            case "Failed Potion":
                StartCoroutine(BlendColor(whiteColor));
                break;

            default:
                StartCoroutine(BlendColor(baseColor));
                break;
        }
    }

    private IEnumerator BlendColor(Color targetColor)
    {
        Color startColor = Renderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < blendDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / blendDuration;

            Renderer.material.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        Renderer.material.color = targetColor;
    }
}
