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
    public class CustomTwoHandedGrabMoveProvider : 
        TwoHandedGrabMoveProvider, 
        ICustomMovementLocomotionProvider, 
        IPresettable<CustomTwoHandedGrabMoveProviderPreset>
    {
        private const string LeftControllerTag = "LeftGameController";
        private const string RightControllerTag = "RightGameController";
        
        [SerializeField] private CustomTwoHandedGrabMoveProviderPreset preset;

        protected override void Awake()
        {
            base.Awake();

            if (Preset == null) return;
            moveFactor = Preset.MoveFactor;
            requireTwoHandsForTranslation = Preset.RequireTwoHandsForTranslation;
            enableRotation = Preset.EnableRotation;
            enableScaling = Preset.EnableScaling;
            
            var leftController = GameObject.FindWithTag(LeftControllerTag);
            if (leftController == null)
                Debug.LogWarning(
                    $"{nameof(CustomTwoHandedGrabMoveProvider)}: No object tagged with '{LeftControllerTag}' has been found. The local transform of the {leftGrabMoveProvider} will be used.");
            else
                leftGrabMoveProvider.controllerTransform = leftController.transform;

            var rightController = GameObject.FindWithTag(RightControllerTag);
            if (rightController == null)
                Debug.LogWarning(
                    $"{nameof(CustomTwoHandedGrabMoveProvider)}: No object tagged with '{RightControllerTag}' has been found. The local transform of the {rightGrabMoveProvider} will be used.");
            else
                rightGrabMoveProvider.controllerTransform = rightController.transform;
        }

        public List<InputActionReference> LeftInputReferences => new(1) { leftGrabMoveProvider.grabMoveAction.reference };
        public List<InputActionReference> RightInputReferences => new(1) { rightGrabMoveProvider.grabMoveAction.reference };
        public LocomotionType LocomotionType => LocomotionType.Movement;
        public string DisplayName => "Grab";
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomTwoHandedGrabMoveProviderPreset Preset => preset;
        public MovementType MovementType => MovementType.Grab;
    }
}
