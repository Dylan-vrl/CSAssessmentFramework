using System;
using CSFramework.Core;
using CSFramework.Presets;
using Unity.XR.CoreUtils;
using UnityEngine;
using Utilities;

namespace CSFramework.Presettables
{
    /// <summary>
    /// Game Handler responsible of controlling the experiment.
    /// </summary>
    public class GameHandler : PresettableMonoBehaviour<GameHandlerPreset>
    {
        public static GameHandler Instance;

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

        [Serializable]
        public enum StateType
        {
            Playing,
            Menu,
            Pause
        }

        private static StateType _state;

        public static StateType State
        {
            get => _state;
            private set
            {
                //This way when the event is called _state is still previous value
                GameStateChanged?.Invoke(value);
                _state = value;
            }
        }

        public static event Action<StateType> GameStateChanged;
        public static event Action GameStarted;
        public static event Action GameEnded;

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
            
            XROrigin = FindObjectOfType<XROrigin>();
            SoundManager.Initialize();
        }

        private void OnEnable()
        {
            State = StateType.Menu;
        }
        
        private void Update()
        {
            if (State != StateType.Playing) return;
            
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
            State = StateType.Playing;
            GameStarted?.Invoke();
        }

        public void EndExperiment()
        {
            SoundManager.PlaySound(SoundManager.Sound.GameEnd);
            State = StateType.Menu;
            GameEnded?.Invoke();
        }

        // Not used yet
        public void PauseExperiment(bool pause)
        {
            State = pause ? StateType.Pause : StateType.Playing;
        }

        public static void ExitApplication()
        {
            Application.Quit();
        }

        public override PresettableCategory GetCategory() => PresettableCategory.Experiment;
    }
}