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
    public class CustomTeleportationProvider : 
        TeleportationProvider, 
        ICustomMovementLocomotionProvider, 
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
        [SerializeField]
        [Tooltip("The reference to the action to select teleport for this controller.")]
        private InputActionProperty leftTeleportSelect;
        
        [Header("Right hand")]
        [SerializeField]
        [Tooltip("The reference to the action to start the teleport aiming mode for this controller.")]
        private InputActionProperty rightTeleportModeActivate;
        [SerializeField]
        [Tooltip("The reference to the action to cancel the teleport aiming mode for this controller.")]
        private InputActionProperty rightTeleportModeCancel;
        [SerializeField]
        [Tooltip("The reference to the action to select teleport for this controller.")]
        private InputActionProperty rightTeleportSelect;
        
        protected override void Awake()
        {
            base.Awake();

            if (Preset == null) return;
            delayTime = Preset.DelayTime;

            // Notify teleportation surfaces that this is their teleportation provider
            var tpInteractables = FindObjectsOfType<BaseTeleportationInteractable>();
            foreach (var tpInteractable in tpInteractables)
            {
                tpInteractable.teleportationProvider = this;
            }
        }

        public List<InputActionReference> LeftInputReferences => new() { leftTeleportModeActivate.reference, leftTeleportModeCancel.reference, leftTeleportSelect.reference };
        public List<InputActionReference> RightInputReferences => new() { rightTeleportModeActivate.reference, rightTeleportModeCancel.reference, rightTeleportSelect.reference };
        public LocomotionType LocomotionType => LocomotionType.Movement;
        public string DisplayName => "Teleportation";
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public CustomTeleportationProviderPreset Preset => preset;
        public MovementType MovementType => MovementType.Teleportation;
    }
}
