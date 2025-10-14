using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class HandBasedAttach : MonoBehaviour
{
    [Header("Attach Points")]
    public Transform leftHandAttach;
    public Transform rightHandAttach;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        // Subscribe to selectEntered to switch attach transform
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
        {
            // Determine hand from interactor's GameObject name
            string handName = controllerInteractor.gameObject.name.ToLower();

            if (handName.Contains("right"))
            {
                grabInteractable.attachTransform = rightHandAttach;
            }
            else
            {
                grabInteractable.attachTransform = leftHandAttach;
            }
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }
}
