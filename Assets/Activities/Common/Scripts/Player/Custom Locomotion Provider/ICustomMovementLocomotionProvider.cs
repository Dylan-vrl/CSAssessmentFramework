using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum MovementType
{
    Disabled,
    Continuous, 
    Teleportation,
    Grab
}

public interface ICustomMovementLocomotionProvider: ICustomLocomotionProvider
{
    public  MovementType MovementType { get; }
}
