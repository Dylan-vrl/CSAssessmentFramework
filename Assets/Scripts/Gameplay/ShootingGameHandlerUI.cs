using DataSaving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameHandler = CSFramework.Presettables.GameHandler;

namespace Gameplay
{
    public class ShootingGameHandlerUI : MonoBehaviour
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
        
        private GameHandler _gM;

        private void OnEnable()
        {
            
            GameHandler.GameStateChanged += OnGameStateChanged;
            // no unsubscribing with anonymous functions
            DataSaver.FolderChanged += (v) => fileNameLabel.text = v;
            DataSaver.DataSaved += (v) => savedNotificationLabel.gameObject.SetActive(v);
        }

        private void Start()
        {
            _gM = GameHandler.Instance;
            endExperimentButton.gameObject.SetActive(false);
            savedNotificationLabel.gameObject.SetActive(false);
            if (timeLabel != null && timeInput != null)
            {
                var t = _gM.ExperimentLength > 0 ? _gM.ExperimentLength.ToString() : "∞";
                timeInput.text = t;
                timeLabel.text = "0.0 / ";
            }
        }

        private void Update()
        {
            if (GameHandler.State == GameHandler.StateType.Playing)
            {
                if (timeLabel != null)
                {
                    
                    timeLabel.text = _gM.PlayTime.ToString("0.0") + " / ";
                }
            }
            
        }

        public void TimerInputChange(string input)
        {
            if (int.TryParse(input, out var i)) _gM.ExperimentLength = i;
        }

        private void OnGameStateChanged(GameHandler.StateType state)
        {
            switch (state)
            {
                case GameHandler.StateType.Menu:
                    savingHolder.SetActive(true);
                    startExperimentButton.gameObject.SetActive(true);
                    endExperimentButton.gameObject.SetActive(false);
                    timeInput.interactable = true;
                    timeLabel.text = (_gM != null ? _gM.PlayTime.ToString("0.0") : 0) + " / ";
                    break;
                
                case GameHandler.StateType.Playing:
                    savingHolder.SetActive(false);
                    startExperimentButton.gameObject.SetActive(false);
                    endExperimentButton.gameObject.SetActive(true);
                    timeInput.interactable = false;
                    var t = _gM.ExperimentLength > 0 ? _gM.ExperimentLength.ToString() : "∞";
                    timeInput.text = t;
                    break;
            }
        }


        private void OnDisable()
        {
            GameHandler.GameStateChanged -= OnGameStateChanged;
        }
    }
}