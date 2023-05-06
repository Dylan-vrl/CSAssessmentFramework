using System;
using System.Linq;
using CSFramework.Presettables;
using Options.Movement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameStateManager.GameState;

namespace Gameplay
{
    public class LocomotionControllerUI : MonoBehaviour
    {
        [Header("Locomotion options")] 
        [SerializeField] private GameObject locomotionHolder;
        [SerializeField] private TMP_Dropdown leftLocDropdown;
        [SerializeField] private TMP_Dropdown rightLocDropdown;
        [SerializeField] private TMP_Dropdown leftTurnDropdown;
        [SerializeField] private TMP_Dropdown rightTurnDropdown;
        [SerializeField] private Toggle leftHandGrabToggle;
        [SerializeField] private Toggle rightHandGrabToggle;

        private LocomotionHandler _locomotionHandler;


        private void OnEnable()
        {
            
            GameStateManager.GameStateChanged += OnGameStateChanged;
        }

        private void Start()
        {
            _locomotionHandler = ExperimentController.Instance.XROrigin.GetComponent<LocomotionHandler>();

            //Movement UI
            if (leftLocDropdown != null && rightLocDropdown != null && leftTurnDropdown != null && rightTurnDropdown != null)
            {
                var movementTypesNames = Enum.GetNames(typeof(LocomotionHandler.MovementType))
                    .Select(i => new TMP_Dropdown.OptionData(i)).ToList();
                leftLocDropdown.options = movementTypesNames;
                leftLocDropdown.value = (int)_locomotionHandler.LeftHandLocomotionType;
                rightLocDropdown.options = movementTypesNames;
                rightLocDropdown.value = (int)_locomotionHandler.RightHandLocomotionType;
                leftTurnDropdown.options = movementTypesNames;
                leftTurnDropdown.value = (int)_locomotionHandler.LeftHandTurnType;
                rightTurnDropdown.options = movementTypesNames;
                rightTurnDropdown.value = (int)_locomotionHandler.RightHandTurnType;
            }

            if (leftHandGrabToggle != null && rightHandGrabToggle != null)
            {
                leftHandGrabToggle.isOn = _locomotionHandler.LeftHandGrabMove;
                rightHandGrabToggle.isOn = _locomotionHandler.RightHandGrabMove;
            }
            
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
        }
    }
}