using System;
using System.Linq;
using CSFramework.Presettables;
using DataSaving;
using Options.Movement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameStateManager.GameState;

namespace Gameplay
{
    public class ExperimentControllerUI : MonoBehaviour
    {
        [Header("Experiment Control options")] 
        [SerializeField] private Button startExperimentButton;
        [SerializeField] private Button endExperimentButton;
        [SerializeField] private TextMeshProUGUI timeLabel;

        [SerializeField] private TMP_InputField timeInput;

        [Header("Locomotion options")] 
        [SerializeField] private GameObject locomotionHolder;
        [SerializeField] private TMP_Dropdown leftLocDropdown;
        [SerializeField] private TMP_Dropdown rightLocDropdown;
        [SerializeField] private TMP_Dropdown leftTurnDropdown;
        [SerializeField] private TMP_Dropdown rightTurnDropdown;
        [SerializeField] private Toggle leftHandGrabToggle;
        [SerializeField] private Toggle rightHandGrabToggle;
        
        [Header("Data Saving")]
        [SerializeField] private GameObject savingHolder;
        [SerializeField] private TextMeshProUGUI fileNameLabel;
        [SerializeField] private TextMeshProUGUI savedNotificationLabel;
        
        private ExperimentController _experimentController;
        private LocomotionHandler _locomotionHandler;


        private void OnEnable()
        {
            
            GameStateManager.GameStateChanged += OnGameStateChanged;
            // no unsubscribing with anonymous functions
            DataSaver.FolderChanged += (v) => fileNameLabel.text = v;
            DataSaver.DataSaved += (v) => savedNotificationLabel.gameObject.SetActive(v);
        }

        private void Start()
        {
            _experimentController = ExperimentController.Instance;
            _locomotionHandler = _experimentController.XROrigin.GetComponent<LocomotionHandler>();
            endExperimentButton.gameObject.SetActive(false);
            savedNotificationLabel.gameObject.SetActive(false);
            if (timeLabel != null && timeInput != null)
            {
                var t = _experimentController.ExperimentLength > 0 ? _experimentController.ExperimentLength.ToString() : "∞";
                timeInput.text = t;
                timeLabel.text = "0.0 / ";
            }

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

        private void Update()
        {
            if (GameStateManager.State == Playing)
            {
                if (timeLabel != null)
                {
                    
                    timeLabel.text = _experimentController.PlayTime.ToString("0.0") + " / ";
                }
            }
            
        }

        public void TimerInputChange(string input)
        {
            if (int.TryParse(input, out var i)) _experimentController.ExperimentLength = i;
        }

        private void OnGameStateChanged(GameStateManager.GameState state)
        {
            switch (state)
            {
                case Menu:
                    savingHolder.SetActive(true);
                    locomotionHolder.SetActive(true);
                    startExperimentButton.gameObject.SetActive(true);
                    endExperimentButton.gameObject.SetActive(false);
                    timeInput.interactable = true;
                    timeLabel.text = (_experimentController != null ? _experimentController.PlayTime.ToString("0.0") : 0) + " / ";
                    break;
                
                case Playing:
                    savingHolder.SetActive(false);
                    locomotionHolder.SetActive(false);
                    startExperimentButton.gameObject.SetActive(false);
                    endExperimentButton.gameObject.SetActive(true);
                    timeInput.interactable = false;
                    var t = _experimentController.ExperimentLength > 0 ? _experimentController.ExperimentLength.ToString() : "∞";
                    timeInput.text = t;
                    break;
            }
        }


        private void OnDisable()
        {
            GameStateManager.GameStateChanged -= OnGameStateChanged;
        }
    }
}