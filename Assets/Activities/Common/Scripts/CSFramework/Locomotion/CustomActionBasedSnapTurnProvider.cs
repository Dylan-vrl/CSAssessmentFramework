using System.Collections.Generic;
using CSFramework.Core;
using CSFramework.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CSFramework.Presettables
{
    public class CustomActionBasedSnapTurnProvider : 
        ActionBasedSnapTurnProvider, 
        ICustomLocomotionProvider, 
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
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomActionBasedSnapTurnProviderPreset Preset => preset;
    }
}
