using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/CustomTeleportationProviderPreset", fileName = "new CustomTeleportationProviderPreset")]
    public class CustomTeleportationProviderPreset : Preset<CustomActionBasedSnapTurnProvider>
    {
        [field: SerializeField,
                Tooltip("The time (in seconds) to delay the teleportation once it is activated. This delay can be used, for example, as time to set a tunneling vignette effect as a VR comfort option.")]
        public float DelayTime { get; private set; } = 0f;
    }
}
