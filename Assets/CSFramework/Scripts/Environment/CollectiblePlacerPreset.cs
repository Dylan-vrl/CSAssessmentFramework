using UnityEngine;
using CSFramework.Core;
using CSFramework.Presettables;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Environment/CollectiblePlacerPreset", fileName = "new CollectiblePlacerPreset")]
    public class CollectiblePlacerPreset: Preset<CollectiblePlacer>
    {
        [field: SerializeField]
        public float Spacing { get; private set; } = 5;
        [field: SerializeField]
        public float NumberOfVisibleCollectibles { get; private set; } = 5;
    }
}