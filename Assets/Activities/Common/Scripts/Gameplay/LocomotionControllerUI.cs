using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static GameStateManager.GameState;

namespace Gameplay
{
    public class LocomotionControllerUI : MonoBehaviour
    {
        [Header("Locomotion options")] 
        [SerializeField] private GameObject locomotionHolder;
        [SerializeField] private LocomotionHandler locomotionHandler;
        [SerializeField] private LocomotionProvider[] locomotionProviderPrefabs;
        [SerializeField] private TMP_Dropdown leftMovDropdown;
        [SerializeField] private TMP_Dropdown rightMovDropdown;
        [SerializeField] private TMP_Dropdown leftRotDropdown;
        [SerializeField] private TMP_Dropdown rightRotDropdown;

        private Dictionary<MovementType, LocomotionProvider> _movementLocomotionProviders = new();
        private Dictionary<RotationType, LocomotionProvider> _rotationLocomotionProviders = new();

        private void Awake()
        {
            _movementLocomotionProviders[MovementType.Disabled] = null;
            _rotationLocomotionProviders[RotationType.Disabled] = null;

            var validProviders = locomotionProviderPrefabs.Where(provider =>
                {
                    var valid = provider != null && provider.TryGetComponent(out ICustomLocomotionProvider _);
                    if (!valid)
                    {
                        Debug.LogWarning(
                            $"{nameof(LocomotionHandler)}: a {nameof(LocomotionProvider)} has been removed from the " +
                            $"left hand list because it doesn't extend {nameof(ICustomLocomotionProvider)}.");
                    }

                    return valid;
                }
            );
            
            foreach (var provider in validProviders)
            {
                if(provider.TryGetComponent<ICustomMovementLocomotionProvider>(out var customMovement))
                {
                    _movementLocomotionProviders.Add(customMovement.MovementType, provider);
                } 
                else if (provider.TryGetComponent<ICustomRotationLocomotionProvider>(out var customRotation))
                {
                    _rotationLocomotionProviders.Add(customRotation.RotationType, provider);
                }
            }

            leftMovDropdown.options = Enum.GetNames(typeof(MovementType)).Select(n => new TMP_Dropdown.OptionData(n)).ToList();
            leftMovDropdown.value = (int)MovementType.Continuous;
            rightMovDropdown.options = Enum.GetNames(typeof(MovementType)).Select(n => new TMP_Dropdown.OptionData(n)).ToList();
            rightMovDropdown.value = (int)MovementType.Disabled;
            
            leftRotDropdown.options = Enum.GetNames(typeof(RotationType)).Select(n => new TMP_Dropdown.OptionData(n)).ToList();
            leftRotDropdown.value = (int)RotationType.Disabled;
            rightRotDropdown.options = Enum.GetNames(typeof(RotationType)).Select(n => new TMP_Dropdown.OptionData(n)).ToList();
            rightRotDropdown.value = (int)RotationType.Snap;
        }

        private void OnEnable()
        {
            GameStateManager.GameStateChanged += OnGameStateChanged;
            leftMovDropdown.onValueChanged.AddListener(OnValueChanged);
            rightMovDropdown.onValueChanged.AddListener(OnValueChanged);
            leftRotDropdown.onValueChanged.AddListener(OnValueChanged);
            rightRotDropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            OnValueChanged(0);
        }

        private void OnValueChanged(int _)
        {
            var left = new List<LocomotionProvider>()
            {
                _movementLocomotionProviders[(MovementType)leftMovDropdown.value],
                _rotationLocomotionProviders[(RotationType)leftRotDropdown.value]
            };
            var right = new List<LocomotionProvider>()
            {
                _movementLocomotionProviders[(MovementType)rightMovDropdown.value],
                _rotationLocomotionProviders[(RotationType)rightRotDropdown.value]
            };
            locomotionHandler.UpdateProviders(left, right);
            
        }

        private void OnGameStateChanged(GameStateManager.GameState state)
        {
            switch (state)
            {
                case Menu:
                    locomotionHolder.SetActive(true);
                    break;
                
                case Playing:
                    locomotionHolder.SetActive(false);
                    break;
            }
        }

        private void OnDisable()
        {
            GameStateManager.GameStateChanged -= OnGameStateChanged;
            leftMovDropdown.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}