using CSFramework.Core;
using UnityEngine;

namespace CSFramework.Temp
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/GameAssetsPreset", fileName = "new GameAssetsPreset")]
    public class GameAssetsPreset: Preset<GameAssets>
    {
        [field: SerializeField]
        public GameAssets.SoundAudioClip[] SoundAudioClips { get; private set; }
    }
}