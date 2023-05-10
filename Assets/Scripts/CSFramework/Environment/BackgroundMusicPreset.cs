using CSFramework.Core;
using CSFramework.Extensions;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Environment/BackgroundMusicPreset", fileName = "new BackgroundMusicPreset")]
    public class BackgroundMusicPreset: Preset<BackgroundMusic>
    {
        // TODO replace with your own fields following this format
        [SerializeField] public Optional<AudioClip> musicStart;
        [SerializeField] public AudioClip musicLoop;
    }
}