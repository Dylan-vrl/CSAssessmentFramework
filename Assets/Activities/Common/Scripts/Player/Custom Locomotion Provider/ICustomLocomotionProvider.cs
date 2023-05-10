using System.Collections.Generic;
using UnityEngine.InputSystem;

public interface ICustomLocomotionProvider
{
    public List<InputActionReference> LeftInputReferences { get; }
    public List<InputActionReference> RightInputReferences { get; }

    public void EnableActions()
    {
        EnableLeftActions();
        EnableRightActions();
    }
    
    public void EnableLeftActions() => LeftInputReferences.ForEach(EnableAction);

    public void EnableRightActions() => RightInputReferences.ForEach(EnableAction);
    
    public void DisableActions()
    {
        DisableLeftActions();
        DisableRightActions();
    }
    
    public void DisableLeftActions() => LeftInputReferences.ForEach(DisableAction);

    public void DisableRightActions() => RightInputReferences.ForEach(DisableAction);

    private static void EnableAction(InputActionReference actionReference)
    {
        var action = GetInputAction(actionReference);
        if (action != null && !action.enabled)
            action.Enable();
    }

    private static void DisableAction(InputActionReference actionReference)
    {
        var action = GetInputAction(actionReference);
        if (action != null && action.enabled)
            action.Disable();
    }

    private static InputAction GetInputAction(InputActionReference actionReference)
    {
#pragma warning disable IDE0031 // Use null propagation -- Do not use for UnityEngine.Object types
        return actionReference != null ? actionReference.action : null;
#pragma warning restore IDE0031
    }
}
