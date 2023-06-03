using CSFramework.Core;
using CSFramework.Presets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CSFramework.Presettables
{
    public class TargetSpawner : PresettableMonoBehaviour<TargetSpawnerPreset>
    {
        [SerializeField] private FollowPath followPath;

        private float _lastStepDistance = 0f;
        private float _currentStep;
        private Terrain _activeTerrain;

        private Vector2 MinMaxStep => Preset.MinMaxStep;
        private Vector2 MinMaxBatchSize => Preset.MinMaxBatchSize;
        private Vector3 MinOffsets => Preset.MinOffsets;
        private Vector3 MaxOffsets => Preset.MaxOffsets;
        private GameObject TargetPrefab => Preset.TargetPrefab;

        private void Awake()
        {
            _currentStep = Random.Range(MinMaxStep.x, MinMaxStep.y);
            _activeTerrain = Terrain.activeTerrain;
        }

        private void Update()
        {
            // Next step reached
            if (followPath.DistanceTravelled - _lastStepDistance >= _currentStep)
            {
                SpawnTargets(Random.Range((int)MinMaxBatchSize.x, (int)MinMaxBatchSize.y));
                _currentStep = Random.Range(MinMaxStep.x, MinMaxStep.y);
                _lastStepDistance = followPath.DistanceTravelled;
            }
        }

        private void SpawnTargets(int count)
        {
            var currDist = followPath.DistanceTravelled;
            var path = followPath.Path;

            for (var i = 0; i < count; i++)
            {
                var nextDist = currDist + Random.Range(MinOffsets.z, MaxOffsets.z);
                var pathPos = path.GetPointAtDistance(nextDist);
                var normal = path.GetNormalAtDistance(nextDist);

                var x = pathPos.x + normal.x * Random.Range(MinOffsets.x, MaxOffsets.x);
                var z = pathPos.z + normal.z * nextDist;

                var spawnPos = new Vector3(
                    x,
                    _activeTerrain.SampleHeight(new Vector3(x, 0f, z)) + _activeTerrain.transform.position.y +
                    normal.y * Random.Range(MinOffsets.y, MaxOffsets.y),
                    z
                );

                Instantiate(TargetPrefab, spawnPos, Quaternion.identity);
            }
        }

        public override PresettableCategory GetCategory() => PresettableCategory.Experiment;
    }
}
