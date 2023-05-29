using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Experiment/TargetSpawnerPreset", fileName = "new TargetSpawnerPreset")]
    public class TargetSpawnerPreset : Preset<TargetSpawner>
    {
        [field: SerializeField,
         Tooltip("Minimal spawn offsets applied to each direction")] 
        public Vector3 MinOffsets { get; private set; } = Vector3.one;
        
        [field: SerializeField,
         Tooltip("Maximal spawn offsets applied to each direction")] 
        public Vector3 MaxOffsets { get; private set; } = Vector3.one;

        [field: SerializeField, 
         Tooltip("Minimal and maximal spawn batch size. A random number between them will be spawned at once")] 
        public Vector2 MinMaxBatchSize { get; private set; } = Vector2.one;
        
        [field: SerializeField, 
         Tooltip("Minimal and maximal spawn step (distance to reach before spawning a new batch. " +
                 "A random number between them will be chosen")] 
        public Vector2 MinMaxStep { get; private set; } = 10 * Vector2.one;
        
        [field: SerializeField, 
         Tooltip("Target to spawn")]
        public GameObject TargetPrefab { get; private set; }
    }
}
