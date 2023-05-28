using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum RotationType
{
    Disabled,
    Continuous, 
    Snap,
}

public interface ICustomRotationLocomotionProvider: ICustomLocomotionProvider
{
    public  RotationType RotationType { get; }
}
