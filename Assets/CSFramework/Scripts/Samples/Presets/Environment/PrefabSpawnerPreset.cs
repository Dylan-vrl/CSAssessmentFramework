using UnityEngine;
using CSFramework.Core;

namespace CSFramework.Samples
{
    [CreateAssetMenu(fileName = "PrefabSpawnerPreset", menuName = "Presets/Environment/PrefabSpawnerPreset")]
    public class PrefabSpawnerPreset : Preset<PrefabSpawner>
    {
        [SerializeField] private SpawnStrategy strategy;

        public SpawnStrategy Strategy => strategy;
    }
}