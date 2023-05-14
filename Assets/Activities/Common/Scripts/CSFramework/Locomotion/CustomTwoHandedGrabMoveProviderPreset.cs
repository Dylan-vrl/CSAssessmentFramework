using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/CustomTwoHandedGrabMoveProviderPreset", fileName = "new CustomTwoHandedGrabMoveProviderPreset")]
    public class CustomTwoHandedGrabMoveProviderPreset : Preset<CustomTwoHandedGrabMoveProvider>
    {
        [field: SerializeField,
                Tooltip("The ratio of actual movement distance to controller movement distance.")] 
        public float MoveFactor { get; private set; } = 2f;
        
        [field: SerializeField,
                Tooltip("Controls whether translation requires both grab move inputs to be active.")] 
        public bool RequireTwoHandsForTranslation { get; private set; } = false;
        
        [field: SerializeField,
                Tooltip("Controls whether to enable yaw rotation of the user.")] 
        public bool EnableRotation { get; private set; } = false;
        
        [field: SerializeField,
                Tooltip("Controls whether to enable uniform scaling of the user.")] 
        public bool EnableScaling { get; private set; } = false;
    }
}
