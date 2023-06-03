using System;
using CSFramework.Core;
using CSFramework.Presets;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using static GameStateManager.GameState;

namespace CSFramework.Presettables
{
    /// <summary>
    /// Game Handler responsible of controlling the experiment.
    /// </summary>
    public class ExperimentController: PresettableMonoBehaviour<ExperimentControllerPreset>
    {
        public static ExperimentController Instance;

        private int _experimentLength;
        private float _lastTimeFMSPlayed;
        
        public XROrigin XROrigin { get; set; }
        public float StartTime { get; private set; }
        public float PlayTime { get; private set; }
        public int ExperimentLength
        {
            get => _experimentLength;
            set => _experimentLength = Mathf.Max(value, 0);
        }

        /// <summary>
        /// This Class executes its awake before others
        /// </summary>
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _experimentLength = Preset.ExperimentLength;
            XROrigin= FindObjectOfType<XROrigin>();
            
            SoundManager.Initialize();
        }

        private void Update()
        {
            if (GameStateManager.State != Playing && GameStateManager.State != Testing) return;
            
            PlayTime += Time.deltaTime;

            if (Preset.PlayFMSPrompt && GameStateManager.State == Playing)
            {
                if (_lastTimeFMSPlayed + Preset.PromptInterval < PlayTime)
                {
                    _lastTimeFMSPlayed = PlayTime;
                    SoundManager.PlaySound(SoundManager.Sound.FMS);
                }
            }

            if (ExperimentLength > 0 && PlayTime > ExperimentLength)
            {
                Debug.Log("Experiment Finished! (timeout)");
                EndExperiment();
            }
        }
        
        public void StartExperiment()
        {
            PlayTime = 0;
            StartTime = Time.time;
            GameStateManager.StartGame();
        }

        public void EndExperiment()
        {
            SoundManager.PlaySound(SoundManager.Sound.GameEnd);
            GameStateManager.EndGame();
        }

        // Not used yet
        public void PauseExperiment(bool pause)
        {
            GameStateManager.PauseGame(pause);
        }

        public static void GoBackToMenu()
        {
            GameStateManager.EndGame();
            SceneManager.LoadScene("Menu");
        }

        public override PresettableCategory GetCategory() => PresettableCategory.Experiment;
    }
}