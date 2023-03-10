using UnityEngine;
using CSFramework.Core;

namespace CSFramework.Samples
{
    [CreateAssetMenu(fileName = "PrefabSpawnerSchedulerPreset", menuName = "Presets/Environment/PrefabSpawnerSchedulerPreset")]
    public class PrefabSpawnerSchedulerPreset : Preset<PrefabSpawnerScheduler>
    {
        [SerializeField] private SpawnStrategy strategy;

        public SpawnStrategy Strategy => strategy;
    }
}