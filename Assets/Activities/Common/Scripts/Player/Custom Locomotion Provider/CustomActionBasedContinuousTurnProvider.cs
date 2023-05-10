using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomActionBasedContinuousTurnProvider : ActionBasedContinuousTurnProvider, ICustomLocomotionProvider
{
    public List<InputActionReference> LeftInputReferences => new(1) { leftHandTurnAction.reference };
    public List<InputActionReference> RightInputReferences => new(1) { rightHandTurnAction.reference };
}
