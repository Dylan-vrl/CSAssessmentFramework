#region

using UnityEngine;
using UnityEngine.Events;

#endregion

// <summary>
/// This class is used for Events that have no argument.
/// </summary>
[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject {
    /// <summary>
    /// The event to transmit.
    /// </summary>
    public event UnityAction OnEventRaised;

    /// <summary>
    /// Invokes the attached event.
    /// </summary>
    public void RaiseEvent() {
        OnEventRaised?.Invoke();
    }
}