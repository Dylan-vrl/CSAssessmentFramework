using UnityEngine;
using Random = UnityEngine.Random;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 minOffsets = Vector3.one;
    [SerializeField] private Vector3 maxOffsets = Vector3.one;
    [SerializeField] private Vector2 minMaxBatchSize = Vector2.one;
    [SerializeField] private Vector2 minMaxStep = 10 * Vector2.one;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private FollowPath followPath;

    private float _lastStepDistance = 0f;
    private float _currentStep;
    private Terrain _activeTerrain;

    private void Awake()
    {
        _currentStep = Random.Range(minMaxStep.x, minMaxStep.y);
        _activeTerrain = Terrain.activeTerrain;
    }

    private void Update()
    {
        // Next step reached
        if (followPath.DistanceTravelled - _lastStepDistance >= _currentStep)
        {
            SpawnTargets(Random.Range((int)minMaxBatchSize.x, (int)minMaxBatchSize.y));
            _currentStep = Random.Range(minMaxStep.x, minMaxStep.y);
            _lastStepDistance = followPath.DistanceTravelled;
        }
    }

    private void SpawnTargets(int count)
    {
        var currDist = followPath.DistanceTravelled;
        var path = followPath.Path;

        for (var i = 0; i < count; i++)
        {
            var nextDist = currDist + Random.Range(minOffsets.z, maxOffsets.z);
            var pathPos = path.GetPointAtDistance(nextDist);
            var normal = path.GetNormalAtDistance(nextDist);

            var x = pathPos.x + normal.x * Random.Range(minOffsets.x, maxOffsets.x);
            var z = pathPos.z + normal.z * nextDist;

            var spawnPos = new Vector3(
                x,
                _activeTerrain.SampleHeight(new Vector3(x, 0f, z)) + _activeTerrain.transform.position.y + normal.y * Random.Range(minOffsets.y, maxOffsets.y),
                z
            );
        
            Instantiate(targetPrefab, spawnPos, Quaternion.identity);
        }
    }
}
