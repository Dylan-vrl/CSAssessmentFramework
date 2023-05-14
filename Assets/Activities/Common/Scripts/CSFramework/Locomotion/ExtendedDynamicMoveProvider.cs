using System.Collections.Generic;
using CSFramework.Core;
using CSFramework.Editor;
using CSFramework.Presets;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

// ReSharper disable InconsistentNaming

namespace CSFramework.Presettables
{
    /// <summary>
    /// This class Is an extended Copy of <c>DynamicMoveProvider</c>. <br/>
    /// Most of the code thus belongs to Unity. Any addition is identified by "Added"<br/>
    /// If interaction toolkit is updated, this class could break.
    /// 
    /// A version of action-based continuous movement that automatically controls the frame of reference that
    /// determines the forward direction of movement based on user preference for each hand.
    /// For example, can configure to use head relative movement for the left hand and controller relative movement for the right hand.
    /// </summary>
    [HideInSetupWindow]
    public class ExtendedDynamicMoveProvider : DynamicMoveProvider, ICustomLocomotionProvider, IPresettable<ExtendedDynamicMoveProviderPreset>
    {
        [SerializeField] private ExtendedDynamicMoveProviderPreset preset;

        private bool fixDownHill;

        private CharacterController characterController;

        protected override void Awake()
        {
            base.Awake();

            if (preset == null) return;
            leftHandMovementDirection = Preset.LeftHandMovementDirection;
            rightHandMovementDirection = Preset.RightHandMovementDirection;
            moveSpeed = Preset.MoveSpeed;
            fixDownHill = Preset.FixDownHill;
            enableStrafe = Preset.EnableStrafe;
        }

        private void Start()
        {
            FindCharacterController();
        }

        protected override void MoveRig(Vector3 translationInWorldSpace)
        {
            //Added floor normal analysis to avoid the stair effect going down slopes
            const float maxDistance = 3;
            var rayDown = new Ray(Quaternion.Euler(0, transform.eulerAngles.y, 0) *characterController.center + transform.position, Vector3.down * maxDistance);
            Physics.Raycast(rayDown, out var hitDownInfo, maxDistance);
            if (fixDownHill)
            {
                if (characterController.velocity.y < 0)
                {
                    var down = hitDownInfo.normal.y < 1 ? Mathf.Sqrt(1 - Mathf.Pow(hitDownInfo.normal.y , 2)) : 0;
                    down *= moveSpeed * Time.deltaTime;
                    translationInWorldSpace.y = -down;
                }
            }
            
            base.MoveRig(translationInWorldSpace);
        }
        
        
        /// <summary>
        /// Method from <see cref="ContinuousMoveProviderBase"/>
        /// For some reason the character controller in the base class is private so we need to find it again.
        /// </summary>
        private void FindCharacterController()
        {
            XROrigin xrOrigin = system.xrOrigin;
            if (xrOrigin == null)
                return;
            
            characterController = xrOrigin.Origin.GetComponent<CharacterController>();
        }

        public List<InputActionReference> LeftInputReferences => new() { leftHandMoveAction.reference };
        public List<InputActionReference> RightInputReferences => new() { rightHandMoveAction.reference };
        public PresettableCategory GetCategory() => PresettableCategory.Locomotion;
        public ExtendedDynamicMoveProviderPreset Preset => preset;
    }
}
