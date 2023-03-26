using UnityEngine;
using CSFramework.Core;
using CSFramework.Extensions;
using CSFramework.Presets;
using PathCreation;

namespace CSFramework.Presettables
{
    public class CollectiblePlacer : Extension<PathCreator, CollectiblePlacerPreset>
    {
        public override PresettableCategory GetCategory() => PresettableCategory.Environment;
        
    }
}