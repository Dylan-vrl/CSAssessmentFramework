using CSFramework.Core;
using CSFramework.Presets;
using Gameplay;
using ScriptableObjects;
using UnityEngine;

namespace CSFramework.Presettables
{
    /// <summary>
    /// Game Handler responsible of controlling the experiment.
    /// </summary>
    public class CoinsController : PresettableMonoBehaviour<CoinsControllerPreset>
    {
        public static CoinsController Instance;
        
        [SerializeField] private CollectiblePickUpSO collectibleEventChannel;
        
        public int NumberOfCollectiblesToPickUp
        {
            get => _numberOfCollectiblesToPickUp;
            set => _numberOfCollectiblesToPickUp = Mathf.Max(value, 0);
        }

        public int PickedUpCollectibles => _pickedUpCollectibles;

        private int _numberOfCollectiblesToPickUp;
        private int _pickedUpCollectibles;

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

            _numberOfCollectiblesToPickUp = Preset.NumberOfCollectiblesToPickUp;
        }

        private void OnEnable()
        {
            if (collectibleEventChannel)
            {
                collectibleEventChannel.OnCollectiblePickup += IncreaseCount;
            }

            GameStateManager.GameStarted += OnExperimentStarted;
        }


        private void OnExperimentStarted()
        {
            _pickedUpCollectibles = 0;
        }

        private void IncreaseCount(Collectible collectible)
        {
            _pickedUpCollectibles++;
            LastCollectiblePos = collectible.transform.position;

            if (Preset.NumberOfCollectiblesToPickUp > 0 && _pickedUpCollectibles >= Preset.NumberOfCollectiblesToPickUp)
            {
                Debug.Log("Finished!");
                ExperimentController.Instance.EndExperiment();
            }
        }


        private void OnDisable()
        {
            if (collectibleEventChannel)
            {
                collectibleEventChannel.OnCollectiblePickup -= IncreaseCount;
            }
            
            GameStateManager.GameStarted -= OnExperimentStarted;
        }

        public override PresettableCategory GetCategory() => PresettableCategory.Experiment;
    }
}