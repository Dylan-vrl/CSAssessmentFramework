using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/FollowPathPreset", fileName = "new FollowPathPreset")]
    public class FollowPathPreset : Preset<TargetSpawner>
    {
         [field: SerializeField] public float Speed { get; private set; } = 5f;
    }
}
