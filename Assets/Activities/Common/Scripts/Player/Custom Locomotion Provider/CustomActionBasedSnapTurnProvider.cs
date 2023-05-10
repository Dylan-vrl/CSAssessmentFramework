using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomActionBasedSnapTurnProvider : ActionBasedSnapTurnProvider, ICustomLocomotionProvider
{
    public List<InputActionReference> LeftInputReferences => new() { leftHandSnapTurnAction.reference };
    public List<InputActionReference> RightInputReferences => new() { rightHandSnapTurnAction.reference };
}
