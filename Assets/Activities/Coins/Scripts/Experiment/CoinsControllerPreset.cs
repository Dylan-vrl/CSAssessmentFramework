using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Experiment/CoinsControllerPreset", fileName = "new CoinsControllerPreset")]
    public class CoinsControllerPreset : Preset<CoinsController>
    {
        [field: SerializeField,
                Tooltip("Number of Collectibles to pick up to finish Experiment, 0 means infinity")]
        public int NumberOfCollectiblesToPickUp { get; private set; } = 10;

        private void OnValidate()
        {
            if (NumberOfCollectiblesToPickUp < 0)
                NumberOfCollectiblesToPickUp = 0;
        }
    }
}
