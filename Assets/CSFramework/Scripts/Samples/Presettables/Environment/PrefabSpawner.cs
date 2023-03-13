using CSFramework.Core;
using UnityEngine;

namespace CSFramework.Samples
{
    public class PrefabSpawner : PresettableMonoBehaviour<PrefabSpawnerPreset>
    {
        public int test;
        public override PresettableCategory Category => PresettableCategory.Vision;
        
        [SerializeField] private GameObject prefab;
    
        public void SpawnPrefab(
            Vector3 position = default,
            Quaternion rotation = default,
            Transform parent = null
        )
        {
            Instantiate(prefab, position, rotation, parent);
        }
    }

    public enum SpawnStrategy
    {
        Random,
        FixedPoint,
        Circular
    }
}
