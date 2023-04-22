using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerReader : MonoBehaviour
{
    [SerializeField] private ActionBasedController _controller;

    public UnityEvent OnSelectStart;
    public UnityEvent OnSelectCanceled;
    public UnityEvent OnActivateStart;
    public UnityEvent OnActivateCanceled;

    private void Awake()
    {
        _controller.selectAction.action.started += OnSelectAction;
        _controller.selectAction.action.canceled += OnSelectAction;

        _controller.activateAction.action.started += OnActivateAction;
        _controller.activateAction.action.canceled += OnActivateAction;
    }

    private void OnSelectAction(InputAction.CallbackContext ctx)
    {
        OnAction(ctx, OnSelectStart, null, OnSelectCanceled);
    }

    private void OnActivateAction(InputAction.CallbackContext ctx)
    {
        OnAction(ctx, OnActivateStart, null, OnActivateCanceled);
    }

    private void OnAction(InputAction.CallbackContext ctx, UnityEvent startEvent = null, UnityEvent performedEvent = null, UnityEvent canceledEvent = null)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                startEvent?.Invoke();
                break;
            case InputActionPhase.Performed:
                performedEvent?.Invoke();
                break;
            case InputActionPhase.Canceled:
                canceledEvent?.Invoke();
                break;
            default:
                break;
        }
    }
}
