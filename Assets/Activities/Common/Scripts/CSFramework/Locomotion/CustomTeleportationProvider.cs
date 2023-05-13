using System.Collections.Generic;
using CSFramework.Core;
using CSFramework.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CSFramework.Presettables
{
    public class CustomTeleportationProvider : 
        TeleportationProvider, 
        ICustomLocomotionProvider, 
        IPresettable<CustomTeleportationProviderPreset>
    {
        [SerializeField] private CustomTeleportationProviderPreset preset;

        [Header("Left hand")]
        [SerializeField]
        [Tooltip("The reference to the action to start the teleport aiming mode for this controller.")]
        private InputActionProperty leftTeleportModeActivate;
        [SerializeField]
        [Tooltip("The reference to the action to cancel the teleport aiming mode for this controller.")]
        private InputActionProperty leftTeleportModeCancel;
        
        [Header("Right hand")]
        [SerializeField]
        [Tooltip("The reference to the action to start the teleport aiming mode for this controller.")]
        private InputActionProperty rightTeleportModeActivate;
        [SerializeField]
        [Tooltip("The reference to the action to cancel the teleport aiming mode for this controller.")]
        private InputActionProperty rightTeleportModeCancel;
        
        protected override void Awake()
        {
            base.Awake();

            if (Preset == null) return;
            delayTime = Preset.DelayTime;
        }

        public List<InputActionReference> LeftInputReferences => new(1) { leftTeleportModeActivate.reference, leftTeleportModeCancel.reference };
        public List<InputActionReference> RightInputReferences => new(1) { rightTeleportModeActivate.reference, rightTeleportModeCancel.reference };
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomTeleportationProviderPreset Preset => preset;
    }
}
