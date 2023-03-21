using CSFramework.Core;
using UnityEngine;

namespace CSFramework.Temp
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/GameHandlerPreset", fileName = "new GameHandlerPreset")]
    public class GameHandlerPreset : Preset<GameHandler>
    {
        [field: SerializeField,
         Tooltip("Number of Collectibles to pick up to finish Experiment, 0 means infinity")]
        public int NumberOfCollectiblesToPickUp { get; private set; } = 10;

        [field: SerializeField,
         Tooltip("Experiment length in seconds, 0 means infinity")]
        public int ExperimentLength { get; private set; } = 30;
        
        [field: SerializeField] 
        public bool PlayFMSPrompt { get; private set; }
        
        [field: SerializeField] 
        public float PromptInterval { get; private set; } = 30f;

        private void OnValidate()
        {
            if (NumberOfCollectiblesToPickUp < 0)
                NumberOfCollectiblesToPickUp = 0;
            if (ExperimentLength < 0)
                ExperimentLength = 0;
        }
    }
}
