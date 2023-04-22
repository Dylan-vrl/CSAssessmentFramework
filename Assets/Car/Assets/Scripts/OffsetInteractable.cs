using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Allows to avoid a XRGrabInteractable to be snapped into the interactor when
/// grabbed. It will keep the same offset as at the beginning of the selection.
/// Be careful to link an attach transform to the object.
/// </summary>
public class OffsetInteractable : XRGrabInteractable
{
    protected override void Awake()
    {
        base.Awake();
        if (attachTransform == null)
            attachTransform = transform;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        MatchAttachPoint(args.interactorObject);
    }

    protected void MatchAttachPoint(IXRInteractor interactor)
    {
        if (IsFirstSelecting(interactor))
        {
            bool IsDirect = interactor is XRDirectInteractor;
            attachTransform.position = IsDirect ? interactor.GetAttachTransform(this).position : transform.position;
            attachTransform.rotation = IsDirect ? interactor.GetAttachTransform(this).rotation : transform.rotation;
        }
    }

    private bool IsFirstSelecting(IXRInteractor interactor)
    {
        return interactor == firstInteractorSelecting;
    }
}
