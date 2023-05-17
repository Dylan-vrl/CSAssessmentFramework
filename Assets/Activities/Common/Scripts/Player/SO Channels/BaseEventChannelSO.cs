using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for scriptable objects used to transmit events carrying exactly one argument
/// </summary>
public abstract class BaseEventChannelSO<T> : ScriptableObject
{
    /// <summary>
    /// The event to transmit.
    /// </summary>
    public event UnityAction<T> OnEventRaised;

    /// <summary>
    /// Invokes the attached event with the given argument.
    /// </summary>
    public virtual void RaiseEvent(T value)
    {
        OnEventRaised?.Invoke(value);
    }
}