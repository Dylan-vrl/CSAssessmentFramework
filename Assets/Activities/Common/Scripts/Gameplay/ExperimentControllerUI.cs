﻿using CSFramework.Presettables;
using DataSaving;
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

        [Header("Data Saving")]
        [SerializeField] private GameObject savingHolder;
        [SerializeField] private TextMeshProUGUI fileNameLabel;
        [SerializeField] private TextMeshProUGUI savedNotificationLabel;
        
        private ExperimentController _experimentController;

        protected virtual void OnEnable()
        {
            
            GameStateManager.GameStateChanged += OnGameStateChanged;
            // no unsubscribing with anonymous functions
            DataSaver.FolderChanged += (v) => fileNameLabel.text = v;
            DataSaver.DataSaved += (v) => savedNotificationLabel.gameObject.SetActive(v);
            
            timeInput.onSubmit.AddListener(TimeInputChange);
        }
        
        private void Start()
        {
            _experimentController = ExperimentController.Instance;
            endExperimentButton.gameObject.SetActive(false);
            savedNotificationLabel.gameObject.SetActive(false);
            if (timeLabel != null && timeInput != null)
            {
                var t = _experimentController.ExperimentLength > 0 ? _experimentController.ExperimentLength.ToString() : "∞";
                timeInput.text = t;
                timeLabel.text = "0.0 / ";
            }
        }

        private void Update()
        {
            if (GameStateManager.State == Playing || GameStateManager.State == Testing)
            {
                if (timeLabel != null)
                {
                    
                    timeLabel.text = _experimentController.PlayTime.ToString("0.0") + " / ";
                }
            }
            
        }

        private void TimeInputChange(string length)
        {
            if (int.TryParse(length, out int value))
            {
                Debug.Log("SENDING EXP LENGTH " + value);
                ExperimentController.Instance.ExperimentLength = value;
            }
        }

        private void OnGameStateChanged(GameStateManager.GameState state)
        {
            switch (state)
            {
                case Menu:
                    savingHolder.SetActive(true);
                    startExperimentButton.gameObject.SetActive(true);
                    endExperimentButton.gameObject.SetActive(false);
                    timeInput.interactable = true;
                    timeLabel.text = (_experimentController != null ? _experimentController.PlayTime.ToString("0.0") : 0) + " / ";
                    break;
                
                case Playing:
                    savingHolder.SetActive(false);
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
            timeInput.onSubmit.RemoveListener(TimeInputChange);
        }
    }
}