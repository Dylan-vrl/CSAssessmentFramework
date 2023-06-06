using CSFramework.Core;
using CSFramework.Extensions;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Vision/PixelizePreset", fileName = "new PixelizePreset")]
    public class PixelizePreset: Preset<Pixelize>
    {
        [field: SerializeField]
        public int ScreenHeight { get; private set; }
    }
}