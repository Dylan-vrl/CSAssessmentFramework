using CSFramework.Core;
using UnityEngine;
using GameAssets = CSFramework.Presettables.GameAssets;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Experiment/GameAssetsPreset", fileName = "new GameAssetsPreset")]
    public class GameAssetsPreset: Preset<GameAssets>
    {
        [field: SerializeField]
        public GameAssets.SoundAudioClip[] SoundAudioClips { get; private set; }
    }
}
