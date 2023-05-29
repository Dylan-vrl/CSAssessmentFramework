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
    public class CustomActionBasedSnapTurnProvider : 
        ActionBasedSnapTurnProvider, 
        ICustomRotationLocomotionProvider, 
        IPresettable<CustomActionBasedSnapTurnProviderPreset>
    {
        [SerializeField] private CustomActionBasedSnapTurnProviderPreset preset;

        protected override void Awake()
        {
            base.Awake();

            if (Preset == null) return;
            turnAmount = Preset.TurnAmount;
            enableTurnAround = Preset.EnableTurnAround;
            enableTurnLeftRight = Preset.EnableTurnLeftRight;
            delayTime = Preset.DelayTime;
        }

        public List<InputActionReference> LeftInputReferences => new(1) { leftHandSnapTurnAction.reference };
        public List<InputActionReference> RightInputReferences => new(1) { rightHandSnapTurnAction.reference };
        public LocomotionType LocomotionType => LocomotionType.Rotation;
        public string DisplayName => "Snap";
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomActionBasedSnapTurnProviderPreset Preset => preset;
        public RotationType RotationType => RotationType.Snap;
    }
}
