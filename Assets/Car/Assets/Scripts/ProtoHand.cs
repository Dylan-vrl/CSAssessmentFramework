using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ProtoHand : MonoBehaviour
{
    [SerializeField] private InputActionReference _gripButton;
    [SerializeField] private InputActionReference _triggerButton;
    [SerializeField] private HandPose _poseOnGrip = HandPose.Grip;
    [SerializeField] private HandPose _poseOnTrigger = HandPose.Point;

    private Animator _animator;

    private int _pointingHash;
    private int _selectHash;
    private int _deselectHash;

    private HandPose _currentPose = HandPose.Idle;

    public UnityEvent OnTriggerOnly;

    private void Awake()
    {
        _pointingHash = Animator.StringToHash("Pointing");
        _selectHash = Animator.StringToHash("Selected");
        _deselectHash = Animator.StringToHash("Deselected");

        _animator = GetComponent<Animator>();

        _gripButton.action.started += SetGripPose;
        _gripButton.action.performed += SetGripPose;
        _gripButton.action.canceled += SetGripPose;

        _triggerButton.action.started += SetTriggerPose;
        _triggerButton.action.performed += SetTriggerPose;
        _triggerButton.action.canceled += SetTriggerPose;
    }

    public void SetGripPose(InputAction.CallbackContext ctx)
    {
        SetPose(ctx, _poseOnGrip);
    }

    public void SetTriggerPose(InputAction.CallbackContext ctx)
    {
        if(SetPose(ctx, _poseOnTrigger))
        {
            OnTriggerOnly?.Invoke();
        }
    }

    private bool SetPose(InputAction.CallbackContext ctx, HandPose pose)
    {
        if (_currentPose == HandPose.Idle && (ctx.started || ctx.performed))
        {
            EnablePose(pose);
            return true;
        }
        else if (ctx.canceled)
        {
            DisablePose(pose);
        }
        return false;
    }

    private void EnablePose(HandPose pose)
    {
        switch (pose)
        {
            case HandPose.Grip:
                _animator.SetTrigger(_selectHash);
                break;
            case HandPose.Point:
                SetPointing(true);
                break;
            case HandPose.Idle:
                SetPointing(false);
                _animator.SetTrigger(_deselectHash);
                break;
            default:
                break;
        }

        _currentPose = pose;
    }

    private void DisablePose(HandPose pose)
    {
        switch (pose)
        {
            case HandPose.Grip:
                _animator.SetTrigger(_deselectHash);
                break;
            case HandPose.Point:
                SetPointing(false);
                break;
            default:
                break;
        }

        _currentPose = HandPose.Idle;
    }

    private void SetPointing(bool enable)
    {
        _animator.SetBool(_pointingHash, enable);
    }

    private enum HandPose { Idle, Grip, Point }
}
