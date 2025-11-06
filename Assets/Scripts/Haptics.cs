using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Haptics : MonoBehaviour
{
    public HapticImpulsePlayer leftHapticsPlayer;
    public HapticImpulsePlayer rightHapticsPlayer;

    public void TriggerHaptics(float amplitude, float duration)
    {
        if (leftHapticsPlayer != null)
            leftHapticsPlayer.SendHapticImpulse(amplitude, duration);

        if (rightHapticsPlayer != null)
            rightHapticsPlayer.SendHapticImpulse(amplitude, duration);
    }
}
