using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportController : MonoBehaviour
{
    public InputActionProperty teleportationActivationAction;
    public XRRayInteractor teleportInteractor;

    void Start()
    {
        
        teleportInteractor.gameObject.SetActive(false);

        
        teleportationActivationAction.action.Enable();

        
        teleportationActivationAction.action.performed += Action_Performed;
        teleportationActivationAction.action.canceled += Action_Canceled;
    }

    private void Action_Performed(InputAction.CallbackContext obj)
    {
        teleportInteractor.gameObject.SetActive(true);
    }

    private void Action_Canceled(InputAction.CallbackContext obj)
    {
        StartCoroutine(JumpOneFrame());
    }

    private void OnDestroy()
    {
        // Unsubscribe events to prevent memory leaks
        teleportationActivationAction.action.performed -= Action_Performed;
        teleportationActivationAction.action.canceled -= Action_Canceled;
    }

    System.Collections.IEnumerator JumpOneFrame()
    {
        // Jump one frame, so the teleport can happen before disabling lol
        yield return null;
        teleportInteractor.gameObject.SetActive(false);

    }
}

