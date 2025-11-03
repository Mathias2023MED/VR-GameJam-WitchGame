using System.Collections;
using UnityEngine;

public class LargeCustomer : PotionEffectCustomer
{
    public SkinnedMeshRenderer skinnedMesh; // assign in inspector
    public string blendShapeName = "BigEye";
    public float enlargementDuration = 1f;

    public Assigner assigner;

    private void Start()
    {
        // Find the Assigner in the scene(make sure you have one)
        assigner = FindFirstObjectByType<Assigner>();
        if (assigner != null)
        {
            skinnedMesh = assigner.skinnedMesh;
        }
        else
        {
            Debug.LogWarning("No Assigner found in the scene!");
        }
    }

    public override void ActivateEffect()
    {
        PlayEnlarge();
    }

    public override void DeactivateEffect()
    {
       
    }

    public void PlayEnlarge()
    {
        int index = skinnedMesh.sharedMesh.GetBlendShapeIndex(blendShapeName);
        if (index >= 0)
            StartCoroutine(EnlargeCoroutine(index));
    }

    IEnumerator EnlargeCoroutine(int index)
    {
        float elapsed = 0f;
        while (elapsed < enlargementDuration)
        {
            elapsed += Time.deltaTime;
            float weight = Mathf.Lerp(0f, 100f, elapsed / enlargementDuration);
            skinnedMesh.SetBlendShapeWeight(index, weight);
            yield return null;
        }
        skinnedMesh.SetBlendShapeWeight(index, 100f);
    }
}
