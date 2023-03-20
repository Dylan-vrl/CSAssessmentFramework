using CSFramework.Core;
using UnityEngine;

namespace CSFramework.Temp
{
    public class GameAssetsPreset: Preset<GameAssets>
    {
        [field: SerializeField]
        public GameAssets.SoundAudioClip[] SoundAudioClips { get; private set; }
    }
}