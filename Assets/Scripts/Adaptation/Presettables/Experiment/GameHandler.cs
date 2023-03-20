using System;
using CSFramework.Core;
using Gameplay;
using ScriptableObjects;
using Unity.XR.CoreUtils;
using UnityEngine;
using Utilities;

namespace CSFramework.Temp
{
    /// <summary>
    /// Game Handler responsible of controlling the experiment.
    /// </summary>
    public class GameHandler : PresettableMonoBehaviour<GameHandlerPreset>
    {
        public static GameHandler Instance;
        
        [SerializeField] private CollectiblePickUpSO collectibleEventChannel;
        
        public XROrigin XROrigin { get; set; }
        
        public float StartTime { get; private set; }

        private int _pickedUpCollectibles;

        private float _lastTimeFMSPlayed;

        private float _playTime;

        public float PlayTime => _playTime;

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
                if (GameStateChanged != null) GameStateChanged(value);
                _state = value;
            }
        }

        public static event Action<StateType> GameStateChanged;

        public static event Action GameStarted;

        public static event Action GameEnded;


        public Vector3 LastCollectiblePos { get; set; } = Vector3.zero;


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
            
            XROrigin = FindObjectOfType<XROrigin>();
            SoundManager.Initialize();
        }

        private void OnEnable()
        {
            if (collectibleEventChannel)
            {
                collectibleEventChannel.OnCollectiblePickup += IncreaseCount;
            }
            
            State = StateType.Menu;
        }


        private void Update()
        {
            if (State == StateType.Playing)
            {
                _playTime = PlayTime + Time.deltaTime;
                
                
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
            
            
        }
        
        
        public void StartExperiment()
        {
            _pickedUpCollectibles = 0;
            _playTime = 0;
            StartTime = Time.time;
            State = StateType.Playing;
            if (GameStarted != null) GameStarted();
        }

        public void EndExperiment()
        {
            SoundManager.PlaySound(SoundManager.Sound.GameEnd);
            State = StateType.Menu;
            if (GameEnded != null) GameEnded();
        }

        // Not used yet
        public void PauseExperiment(bool pause)
        {
            State = pause ? StateType.Pause : StateType.Playing;
        }


        private void IncreaseCount(Collectible collectible)
        {
            _pickedUpCollectibles++;
            LastCollectiblePos = collectible.transform.position;

            if (Preset.NumberOfCollectiblesToPickUp > 0 && _pickedUpCollectibles >= Preset.NumberOfCollectiblesToPickUp)
            {
                Debug.Log("Finished!");
                EndExperiment();
            }
        }

        public static void ExitApplication()
        {
            Application.Quit();
        }
        

        private void OnDisable()
        {
            if (collectibleEventChannel)
            {
                collectibleEventChannel.OnCollectiblePickup -= IncreaseCount;
            }
        }

        public override PresettableCategory Category => PresettableCategory.Experiment;
    }
}
