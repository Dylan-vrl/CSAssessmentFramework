using System.Collections.Generic;
using CSFramework.Core;
using CSFramework.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CSFramework.Presettables
{
    public class CustomTwoHandedGrabMoveProvider : 
        TwoHandedGrabMoveProvider, 
        ICustomLocomotionProvider, 
        IPresettable<CustomTwoHandedGrabMoveProviderPreset>
    {
        [SerializeField] private CustomTwoHandedGrabMoveProviderPreset preset;

        protected override void Awake()
        {
            base.Awake();

            if (Preset == null) return;
            moveFactor = Preset.MoveFactor;
            requireTwoHandsForTranslation = Preset.RequireTwoHandsForTranslation;
            enableRotation = Preset.EnableRotation;
            enableScaling = Preset.EnableScaling;
        }

        public List<InputActionReference> LeftInputReferences => new(1) { leftGrabMoveProvider.grabMoveAction.reference };
        public List<InputActionReference> RightInputReferences => new(1) { rightGrabMoveProvider.grabMoveAction.reference };
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomTwoHandedGrabMoveProviderPreset Preset => preset;
    }
}
