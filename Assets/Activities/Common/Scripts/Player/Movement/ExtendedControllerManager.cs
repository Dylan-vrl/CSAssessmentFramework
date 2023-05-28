using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
// ReSharper disable InconsistentNaming

namespace Player.Movement
{
    /// <summary>
    /// This class Is an extended Copy of <see cref="ActionBasedControllerManager"/>.
    /// If interaction toolkit is updated, this class could break.
    /// Use this class to mediate the controllers and their associated interactors and input actions under different interaction states.
    /// </summary>
    [AddComponentMenu("XR/Extended Controller Manager")]
    [DefaultExecutionOrder(KUpdateOrder)]
    public class ExtendedControllerManager : MonoBehaviour
    {
        /// <summary>
        /// Order when instances of type <see cref="ActionBasedControllerManager"/> are updated.
        /// </summary>
        /// <remarks>
        /// Executes before controller components to ensure input processors can be attached
        /// to input actions and/or bindings before the controller component reads the current
        /// values of the input actions.
        /// </remarks>
        public const int KUpdateOrder = XRInteractionUpdateOrder.k_Controllers - 1;
        
        [Space]
        [Header("Interactors")]

        [SerializeField]
        [Tooltip("The GameObject containing the interactor used for direct manipulation.")]
        private XRDirectInteractor directInteractor;
        public XRDirectInteractor DirectInteractor => directInteractor;

        [SerializeField]
        [Tooltip("The GameObject containing the interactor used for distant/ray manipulation.")]
        private XRRayInteractor m_RayInteractor;

        [SerializeField]
        [Tooltip("Whether the ray Interactor can be enabled or not.")]
        private bool canRayInteract;
        
        public bool CanRayInteract
        {
            get => canRayInteract;
            set => canRayInteract = value;
        }

        [SerializeField]
        [Tooltip("The GameObject containing the interactor used for teleportation.")]
        private XRRayInteractor m_TeleportInteractor;

        [Space]
        [Header("Controller Actions")]

        [SerializeField]
        [Tooltip("The reference to the action to start the teleport aiming mode for this controller.")]
        private InputActionReference teleportModeActivate;
        

        [SerializeField]
        [Tooltip("The reference to the action to cancel the teleport aiming mode for this controller.")]
        private InputActionReference teleportModeCancel;

        [SerializeField]
        [Tooltip("The reference to the action of continuous turning the XR Origin with this controller.")]
        private InputActionReference turn;

        [SerializeField]
        [Tooltip("The reference to the action of snap turning the XR Origin with this controller.")]
        private InputActionReference snapTurn;

        [SerializeField]
        [Tooltip("The reference to the action of moving the XR Origin with this controller.")]
        private InputActionReference move;

        private bool m_DirectHover;
        private bool m_DirectSelect;
        private bool m_Teleporting;

        public InputActionReference TeleportModeActivate => teleportModeActivate;
        
        public InputActionReference TeleportModeCancel => teleportModeCancel;

        public InputActionReference Turn => turn;

        public InputActionReference SnapTurn => snapTurn;

        public InputActionReference Move => move;
        
        // For our input mediation, we are enforcing a few rules between direct, ray, and teleportation interaction:
        // 1. If the Teleportation Ray is engaged, the Direct and Ray interactors are disabled
        // 2. If the Direct interactor is not idle (hovering or select), the ray interactor is disabled
        // 3. If the Ray interactor is selecting, all locomotion controls are disabled (teleport ray and snap controls) to prevent input collision
        void SetupInteractorEvents()
        {
            if (directInteractor != null)
            {
                directInteractor.hoverEntered.AddListener(DirectHoverEntered);
                directInteractor.hoverExited.AddListener(DirectHoverExited);
                directInteractor.selectEntered.AddListener(DirectSelectEntered);
                directInteractor.selectExited.AddListener(DirectSelectExited);
            }

            if (m_RayInteractor != null)
            {
                m_RayInteractor.selectEntered.AddListener(RaySelectEntered);
                m_RayInteractor.selectExited.AddListener(RaySelectExited);
            }

            if (teleportModeActivate != null && teleportModeCancel != null)
            {
                var teleportModeAction = GetInputAction(teleportModeActivate);
                var cancelTeleportModeAction = GetInputAction(teleportModeCancel);
                teleportModeAction.performed += StartTeleport;
                teleportModeAction.canceled += CancelTeleport;
                cancelTeleportModeAction.performed += CancelTeleport;
            }
        }

        void TeardownInteractorEvents()
        {
            if (directInteractor != null)
            {
                directInteractor.hoverEntered.RemoveListener(DirectHoverEntered);
                directInteractor.hoverExited.RemoveListener(DirectHoverExited);
                directInteractor.selectEntered.RemoveListener(DirectSelectEntered);
                directInteractor.selectExited.RemoveListener(DirectSelectExited);
            }

            if (m_RayInteractor != null)
            {
                m_RayInteractor.selectEntered.RemoveListener(RaySelectEntered);
                m_RayInteractor.selectExited.RemoveListener(RaySelectExited);
            }

            if (teleportModeActivate != null && teleportModeCancel != null)
            {
                var teleportModeAction = GetInputAction(teleportModeActivate);
                var cancelTeleportModeAction = GetInputAction(teleportModeCancel);
                teleportModeAction.performed -= StartTeleport;
                teleportModeAction.canceled -= CancelTeleport;
                cancelTeleportModeAction.performed -= CancelTeleport;
            }
        }

        void StartTeleport(InputAction.CallbackContext obj)
        {
            m_Teleporting = true;
            if (m_TeleportInteractor != null)
                m_TeleportInteractor.gameObject.SetActive(true);
            RayInteractorUpdate();
        }

        void CancelTeleport(InputAction.CallbackContext obj)
        {
            m_Teleporting = false;
            // Do not deactivate the teleport interactor in this callback.
            // We delay turning off the teleport interactor in this callback so that
            // the teleport interactor has a chance to complete the teleport if needed.
            // OnAfterInteractionEvents will handle deactivating its GameObject.
            RayInteractorUpdate();
        }

        void DirectHoverEntered(HoverEnterEventArgs args)
        {
            m_DirectHover = true;
            DirectInteractorUpdate();
        }

        void DirectHoverExited(HoverExitEventArgs args)
        {
            m_DirectHover = false;
            DirectInteractorUpdate();
        }

        void DirectSelectEntered(SelectEnterEventArgs args)
        {
            m_DirectSelect = true;
            DirectInteractorUpdate();
        }

        void DirectSelectExited(SelectExitEventArgs args)
        {
            m_DirectSelect = false;
            DirectInteractorUpdate();
        }

        void DirectInteractorUpdate()
        {
            RayInteractorUpdate();
        }

        void RayInteractorUpdate()
        {
            if (m_RayInteractor != null)
                m_RayInteractor.gameObject.SetActive(CanRayInteract && !(m_DirectHover || m_DirectSelect || m_Teleporting));
        }

        void RaySelectEntered(SelectEnterEventArgs args)
        {
            // Disable direct selection
            if (directInteractor != null)
                directInteractor.gameObject.SetActive(false);

            // Disable locomotion and turn actions
            DisableLocomotionAndTurnActions();
        }

        void RaySelectExited(SelectExitEventArgs args)
        {
            // Enable direct selection
            if (directInteractor != null)
                directInteractor.gameObject.SetActive(true);
            EnableLocomotionAndTurnActions();
        }

        protected virtual void Awake()
        {
            // Start the coroutine that executes code after the Update phase (during yield null).
            // This routine is started during Awake to ensure the code after
            // the first yield will execute after Update but still on the first frame.
            // If started in Start, Unity would not resume execution until the second frame.
            // See https://docs.unity3d.com/Manual/ExecutionOrder.html
            StartCoroutine(OnAfterInteractionEvents());
            DirectInteractorUpdate();
            RayInteractorUpdate();
        }

        protected virtual void OnEnable()
        {
            if (m_TeleportInteractor != null)
                m_TeleportInteractor.gameObject.SetActive(false);

            SetupInteractorEvents();
        }

        protected void OnDisable()
        {
            TeardownInteractorEvents();
        }
        
        IEnumerator OnAfterInteractionEvents()
        {
            // Avoid comparison to null each frame since that operation is somewhat expensive
            if (m_TeleportInteractor == null)
                yield break;

            while (true)
            {
                // Yield so this coroutine is resumed after the teleport interactor
                // has a chance to process its select interaction event.
                yield return null;

                if (!m_Teleporting && m_TeleportInteractor.gameObject.activeSelf)
                    m_TeleportInteractor.gameObject.SetActive(false);
            }
        }

        private void DisableLocomotionAndTurnActions()
        {
            DisableAction(teleportModeActivate);
            DisableAction(teleportModeCancel);
            DisableAction(move);
            DisableAction(snapTurn);
            DisableAction(turn);
        }
        
        private void EnableLocomotionAndTurnActions()
        {
            var en = false;
            if (LocomotionHandler.ActionRefStates.TryGetValue(teleportModeActivate, out en))
                EnableAction(teleportModeActivate, en);
            if (LocomotionHandler.ActionRefStates.TryGetValue(teleportModeCancel, out en))
                EnableAction(teleportModeCancel, en);
            if (LocomotionHandler.ActionRefStates.TryGetValue(move, out en))
                EnableAction(move, en);
            if (LocomotionHandler.ActionRefStates.TryGetValue(snapTurn, out en))
                EnableAction(snapTurn, en);
            if (LocomotionHandler.ActionRefStates.TryGetValue(turn, out en))
                EnableAction(turn, en);
        }

        private static void EnableAction(InputActionReference actionReference, bool enabled)
        {
            if (enabled)
                EnableAction(actionReference);
            else 
                DisableAction(actionReference);
        }
        
        private static void EnableAction(InputActionReference actionReference)
        {
            InputAction action = GetInputAction(actionReference);
            if (action != null && !action.enabled)
                action.Enable();
        }

        private static void DisableAction(InputActionReference actionReference)
        {
            InputAction action = GetInputAction(actionReference);
            if (action != null && action.enabled)
                action.Disable();
        }

        private static InputAction GetInputAction(InputActionReference actionReference)
        {
#pragma warning disable IDE0031 // Use null propagation -- Do not use for UnityEngine.Object types
            return actionReference != null ? actionReference.action : null;
#pragma warning restore IDE0031
        }
    }
}
