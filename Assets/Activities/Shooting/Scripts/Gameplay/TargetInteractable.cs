using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetInteractable : XRBaseInteractable
{
    [SerializeField] 
    private int score = 1;
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
