using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/CustomActionBasedSnapTurnProviderPreset", fileName = "new CustomActionBasedSnapTurnProviderPreset")]
    public class CustomActionBasedSnapTurnProviderPreset : Preset<CustomActionBasedSnapTurnProvider>
    {
        [field: SerializeField,
                Tooltip("The number of degrees clockwise to rotate when snap turning clockwise.")] 
        public float TurnAmount { get; private set; } = 60f;
        
        [field: SerializeField,
                Tooltip("Controls whether to enable left & right snap turns.")] 
        public bool EnableTurnLeftRight { get; private set; } = true;
        
        [field: SerializeField, 
                Tooltip("Controls whether to enable 180° snap turns.")] 
        public bool EnableTurnAround { get; private set; } = true;
        
        [field: SerializeField,
                Tooltip("The time (in seconds) to delay the first turn after receiving initial input for the turn.")]
        public float DelayTime { get; private set; } = 0f;
    }
}
