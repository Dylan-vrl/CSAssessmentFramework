using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Experiment/ExperimentControllerPreset", fileName = "new ExperimentControllerPreset")]
    public class ExperimentControllerPreset : Preset<ExperimentController>
    {
        [field: SerializeField,
                Tooltip("Experiment length in seconds, 0 means infinity")]
        public int ExperimentLength { get; private set; } = 30;
        
        [field: SerializeField] 
        public bool PlayFMSPrompt { get; private set; }
        
        [field: SerializeField] 
        public float PromptInterval { get; private set; } = 30f;

        private void OnValidate()
        {
            if (ExperimentLength < 0)
                ExperimentLength = 0;
        }
    }
}
