using CSFramework.Core;
using CSFramework.Presets;
using Unity.XR.CoreUtils;
using UnityEngine;
using Utilities;
using static GameStateManager.GameState;

namespace CSFramework.Presettables
{
    /// <summary>
    /// Game Handler responsible of controlling the experiment.
    /// </summary>
    public class ExperimentController: PresettableMonoBehaviour<GameHandlerPreset>
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
            DontDestroyOnLoad(gameObject);

            _experimentLength = Preset.ExperimentLength;
            XROrigin= FindObjectOfType<XROrigin>();
            
            SoundManager.Initialize();
        }

        private void Update()
        {
            if (GameStateManager.State != Playing) return;
            
            PlayTime += Time.deltaTime;

            if (Preset.PlayFMSPrompt)
            {
                if (_lastTimeFMSPlayed + Preset.PromptInterval < PlayTime)
                {
                    _lastTimeFMSPlayed = PlayTime;
                    SoundManager.PlaySound(SoundManager.Sound.FMS);
                }
            }

            if (Preset.ExperimentLength > 0 && PlayTime > Preset.ExperimentLength)
            {
                Debug.Log("Experiment Finished! (timeout)");
                EndExperiment();
            }
        }
        
        public void StartExperiment()
        {
            PlayTime = 0;
            StartTime = Time.time;
            GameStateManager.StartExperiment();
        }

        public void EndExperiment()
        {
            SoundManager.PlaySound(SoundManager.Sound.GameEnd);
            GameStateManager.EndExperiment();
        }

        // Not used yet
        public void PauseExperiment(bool pause)
        {
            GameStateManager.PauseExperiment(pause);
        }

        public static void ExitApplication()
        {
            Application.Quit();
        }

        public override PresettableCategory GetCategory() => PresettableCategory.Experiment;
    }
}