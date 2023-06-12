using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player.Character
{
    /// <summary>
    /// Helper for controller visuals based on actions. Updating material and controlling animation.
    /// </summary>
    public class ControllerHelper : MonoBehaviour
    {
        [FormerlySerializedAs("interactionRelatedAction")]
        [Header("Actions related to interaction and Locomotion")]
        [SerializeField] private InputActionReference interactionAction;
        [SerializeField] private InputActionReference locomotionRelatedAction;

        [Header("Actions with Button Values, for animation")]
        [SerializeField] private InputActionReference triggerValue;
        [SerializeField] private InputActionReference gripValue;
        [SerializeField] private InputActionReference primaryButton;
        [SerializeField] private InputActionReference secondaryButton;
        [SerializeField] private InputActionReference joystickValue;

        [Header("Materials")]
        [SerializeField] private Material enabledMaterial;
        [SerializeField] private Material disabledMaterial;

        [Header("Button Renderers")]
        [SerializeField] private Renderer triggerRenderer;
        [SerializeField] private Renderer gripRenderer;
        [SerializeField] private Renderer primaryBRenderer;
        [SerializeField] private Renderer secondaryBRenderer;
        [SerializeField] private Renderer joystickRenderer;
        

        /// <summary>
        /// The animator component that contains the controller animation controller for animating buttons and triggers.
        /// </summary>
        private Animator _animator;

        private static readonly int JoyX = Animator.StringToHash("Joy X");
        private static readonly int JoyY = Animator.StringToHash("Joy Y");
        private static readonly int PrimaryB = Animator.StringToHash("Primary B");
        private static readonly int Property = Animator.StringToHash("Secondary B");
        private static readonly int Grip = Animator.StringToHash("Grip");
        private static readonly int Trigger = Animator.StringToHash("Trigger");

        /// <summary>
        /// Naming the input action names
        /// </summary>
        private static readonly string JoystickName = UnityEngine.XR.CommonUsages.primary2DAxis.name;
        private static readonly string GripName = UnityEngine.XR.CommonUsages.grip.name;
        private static readonly string TriggerName = UnityEngine.XR.CommonUsages.trigger.name;
        private static readonly string PrimaryButtonName = UnityEngine.XR.CommonUsages.primaryButton.name;
        private static readonly string SecondaryButtonName = UnityEngine.XR.CommonUsages.secondaryButton.name;
        
        private Dictionary<string, HashSet<InputAction>> _actions;
        private Dictionary<string, Renderer> _renderers;


        private void OnEnable()
        {
            GameStateManager.GameStarted += OnActionsModified;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            // setting up the renderers dictionary to simplify links
            _renderers = new Dictionary<string, Renderer>();
            _renderers.Add(JoystickName, joystickRenderer);
            _renderers.Add(GripName, gripRenderer);
            _renderers.Add(TriggerName, triggerRenderer);
            _renderers.Add(PrimaryButtonName, primaryBRenderer);
            _renderers.Add(SecondaryButtonName, secondaryBRenderer);

            _actions = new();
            _actions.Add(JoystickName, new HashSet<InputAction>());
            _actions.Add(GripName, new HashSet<InputAction>());
            _actions.Add(TriggerName, new HashSet<InputAction>());
            _actions.Add(PrimaryButtonName, new HashSet<InputAction>());
            _actions.Add(SecondaryButtonName, new HashSet<InputAction>());
            
            // we only add a single action instead of all in the actionmap since interactions are more difficult to disable.
            AddActiveInputActions(interactionAction.action);
            
            AddActiveInputActions(locomotionRelatedAction.action.actionMap);
            OnActionsModified();
            
        }

        private void AddActiveInputActions(InputActionMap inputActionMap)
        {
            foreach (InputAction inputAction in inputActionMap)
            {
                AddActiveInputActions(inputAction);
            }
        }

        /// <summary>
        /// Adds all input actions that have a binding to an element of the controller that interests us to the <see cref="_actions"/> dictionary.
        /// </summary>
        /// <param name="inputAction">The input Action for which we want to check the bindings.</param>
        private void AddActiveInputActions(InputAction inputAction)
        {
            foreach (InputBinding binding in inputAction.bindings)
            {
                var bindingPath = binding.effectivePath;
                var bindingName = bindingPath[(bindingPath.LastIndexOf("/", StringComparison.Ordinal) + 1)..];

                foreach (var key in _actions.Keys)
                {
                    if (bindingName.Contains(key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _actions[key].Add(inputAction);
                    }
                }
            }
        }

        private void OnActionsModified()
        {
            foreach (var buttonName in _renderers.Keys)
            {
                UpdateMaterial(_renderers[buttonName],_actions[buttonName]);
            }
        }

        private void UpdateMaterial(Renderer buttonRenderer, HashSet<InputAction> relatedActions)
        {
            if (buttonRenderer != null && enabledMaterial != null && disabledMaterial != null)
            {
                if (relatedActions.Any(r => r.enabled))
                {
                    Debug.Log($"enabled {buttonRenderer.name} from {string.Join(", ",relatedActions.Where(i => i.enabled))}");
                    if (buttonRenderer.material != enabledMaterial)
                    {
                        buttonRenderer.material = enabledMaterial;
                    }
                }
                else
                {
                    
                    if (buttonRenderer.material != disabledMaterial)
                    {
                        buttonRenderer.material = disabledMaterial;
                    }
                }
            }
        }


        private void Update()
        {
            AnimateButtons();
        }

        private void AnimateButtons()
        {
            if (_animator != null)
            {
                if (joystickValue != null)
                {
                    _animator.SetFloat(JoyX, joystickValue.action.ReadValue<Vector2>().x);
                    _animator.SetFloat(JoyY, joystickValue.action.ReadValue<Vector2>().y);
                }

                if (triggerValue != null)
                {
                    _animator.SetFloat(Trigger, triggerValue.action.ReadValue<float>());
                }

                if (gripValue != null)
                {
                    _animator.SetFloat(Grip, gripValue.action.ReadValue<float>());
                }

                if (primaryButton != null)
                {
                    _animator.SetFloat(PrimaryB, primaryButton.action.IsPressed() ? 1 : 0);
                }

                if (secondaryButton != null)
                {
                    _animator.SetFloat(Property, secondaryButton.action.IsPressed() ? 1 : 0);
                }
            }
        }


        private void OnDisable()
        {
            GameStateManager.GameStarted -= OnActionsModified;
        }
    }
}
