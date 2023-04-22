using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Mathf;

public class WheelInteractable : XRBaseInteractable
{
    [SerializeField][Range(0f, 180f)] private float _maxRotationOffset;
    [SerializeField] private Transform _anchor;
    [SerializeField] private bool _negateXAxis;

    private Rigidbody _rigidbody;

    private float _initialWheelRotation;
    private float _lastFrameInteractorAngle;
    private Vector3 _rightReference;
    private IXRSelectInteractor _currentInteractor;

    private Vector3 _targetRotation;

    public float MaxRotationOffset => _maxRotationOffset;
    public Transform Anchor => _anchor;

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
            Debug.LogError("Wheel Interactable does not have a required Rigidbody.", this);

        _rightReference = _negateXAxis ? -_anchor.right : _anchor.right;
        _initialWheelRotation = _anchor.localEulerAngles.z;
    }

    /// <inheritdoc />
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if (_currentInteractor != null) return;

        _currentInteractor = args.interactorObject;
        _targetRotation = _anchor.localEulerAngles;

        var handPosition = args.interactorObject.transform.position;
        var localHandPos = handPosition - _anchor.position;
        var _initialInteractorAngle = Vector3.SignedAngle(_rightReference, localHandPos, _anchor.forward);
        _lastFrameInteractorAngle = _initialInteractorAngle;
    }

    /// <inheritdoc />
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        switch (updatePhase)
        {
            // During Fixed update we want to perform any physics-based updates (e.g., Kinematic or VelocityTracking).
            case XRInteractionUpdateOrder.UpdatePhase.Fixed:

                break;

            // During Dynamic update we want to perform any Transform-based manipulation (e.g., Instantaneous).
            case XRInteractionUpdateOrder.UpdatePhase.Dynamic:
                if (isSelected)
                {
                    var interactor = interactorsSelecting[0];
                    UpdateTarget(interactor);
                    UpdateRotation();
                }

                break;

            // During OnBeforeRender we want to perform any last minute Transform position changes before rendering (e.g., Instantaneous).
            case XRInteractionUpdateOrder.UpdatePhase.OnBeforeRender:
                if (isSelected)
                {
                    var interactor = interactorsSelecting[0];
                    UpdateTarget(interactor);
                    UpdateRotation();
                }

                break;

            // Late update is only used to handle detach as late as possible.
            case XRInteractionUpdateOrder.UpdatePhase.Late:

                break;
        }

        }

    /// <inheritdoc />
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        _currentInteractor = null;
    }

    void UpdateRotation()
    {
        _anchor.localRotation = Quaternion.Euler(_targetRotation);
    }

    void UpdateTarget(IXRInteractor interactor)
    {
        var handPosition = interactor.transform.position;
        var localHandPos = handPosition - _anchor.position;
        var signedAngle = Vector3.SignedAngle(_rightReference, localHandPos, _anchor.forward);

        var angle = signedAngle;
        if(Sign(angle) != Sign(_lastFrameInteractorAngle))
        {
            if (_lastFrameInteractorAngle < 0)
                _lastFrameInteractorAngle += 360;
        }

        float zRotOffset = angle - _lastFrameInteractorAngle;

        float nextZRot = _targetRotation.z + zRotOffset;
        float comparisonZRot = nextZRot;
        if (nextZRot > 180)
            comparisonZRot = nextZRot - 360;
        else if (nextZRot < -180)
            comparisonZRot = nextZRot + 360;

        if (comparisonZRot >= _initialWheelRotation - _maxRotationOffset && comparisonZRot <= _initialWheelRotation + _maxRotationOffset)
            _targetRotation = new Vector3(_anchor.localEulerAngles.x, _anchor.localEulerAngles.y, nextZRot);

        _lastFrameInteractorAngle = signedAngle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        var rotationOffsetRad = _maxRotationOffset * Deg2Rad;

        var negDir = new Vector3(Sin(-rotationOffsetRad), Cos(-rotationOffsetRad), 0f);
        var posDir = new Vector3(Sin(rotationOffsetRad), Cos(rotationOffsetRad), 0f);
        Gizmos.DrawRay(_anchor.position, negDir);
        Gizmos.DrawRay(_anchor.position, posDir);
    }
}
