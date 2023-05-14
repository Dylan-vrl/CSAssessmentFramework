using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/CustomTwoHandedGrabMoveProviderPreset", fileName = "new CustomTwoHandedGrabMoveProviderPreset")]
    public class CustomTwoHandedGrabMoveProviderPreset : Preset<CustomTwoHandedGrabMoveProvider>
    {
        [field: SerializeField] public float MoveFactor { get; private set; } = 2f;
        [field: SerializeField] public bool RequireTwoHandsForTranslation { get; private set; } = false;
        [field: SerializeField] public bool EnableRotation { get; private set; } = false;
        [field: SerializeField] public bool EnableScaling { get; private set; } = false;
    }
}
