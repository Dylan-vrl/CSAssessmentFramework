using UnityEngine;
using CSFramework.Core;
using CSFramework.Presettables;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Environment/CollectiblePlacerPreset", fileName = "new CollectiblePlacerPreset")]
    public class CollectiblePlacerPreset: Preset<CollectiblePlacer>
    {
        // TODO remove this field and create your own fields following the exact same format
        [field: SerializeField]
        public int Field { get; private set; }
    	
    }
}