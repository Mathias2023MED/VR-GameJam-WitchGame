using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportController : MonoBehaviour
{
    public InputActionProperty teleportationActivationAction;
    public XRRayInteractor teleportInteractor;

    void Start()
    {
        // Typo fix: SetActive (not SetAcive)
        teleportInteractor.gameObject.SetActive(false);

        // Typo fix: Enable (not Enalbe)
        teleportationActivationAction.action.Enable();

        // Typo fix: performed (not preformed)
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
        yield return null;
        teleportInteractor.gameObject.SetActive(false);

    }
}

