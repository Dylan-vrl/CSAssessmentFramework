using System.Collections.Generic;
using CSFramework.Core;
using CSFramework.Editor;
using CSFramework.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CSFramework.Presettables
{
    [HideInSetupWindow]
    public class CustomActionBasedContinuousTurnProvider : 
        ActionBasedContinuousTurnProvider, 
        ICustomLocomotionProvider, 
        IPresettable<CustomActionBasedContinuousTurnProviderPreset>
    {
        [SerializeField] private CustomActionBasedContinuousTurnProviderPreset preset;

        protected override void Awake()
        {
            base.Awake();

            if (Preset == null) return;
            turnSpeed = Preset.TurnSpeed;
        }

        public List<InputActionReference> LeftInputReferences => new(1) { leftHandTurnAction.reference };
        public List<InputActionReference> RightInputReferences => new(1) { rightHandTurnAction.reference };
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomActionBasedContinuousTurnProviderPreset Preset => preset;
    }
}
