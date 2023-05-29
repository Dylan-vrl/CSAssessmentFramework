using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/CustomActionBasedContinuousTurnProviderPreset", fileName = "new CustomActionBasedContinuousTurnProviderPreset")]
    public class CustomActionBasedContinuousTurnProviderPreset : Preset<CustomActionBasedContinuousTurnProvider>
    {
        [field: SerializeField] public float TurnSpeed { get; private set; } = 60f;
    }
}
