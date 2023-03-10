using UnityEngine;

namespace CSFramework.Samples
{
    public class PrefabSpawner : MonoBehaviour
    {
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
