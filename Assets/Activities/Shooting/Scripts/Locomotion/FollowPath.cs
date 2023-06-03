using System;
using CSFramework.Core;
using CSFramework.Presets;
using PathCreation;
using UnityEngine;

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class FollowPath : PresettableMonoBehaviour<FollowPathPreset>
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private EndOfPathInstruction endOfPathInstruction;
    [SerializeField] private float yOffset = 1f;

    public float Speed { get; set; }

    public float DistanceTravelled { get; private set; }
    public VertexPath Path => pathCreator.path;
    private bool _isFollowing = false;
    private Terrain _activeTerrain;

    private void Awake()
    {
        Speed = Preset.Speed;
    }

    private void OnEnable()
    {
        GameStateManager.GameStarted += StartFollowing;
        GameStateManager.GameEnded += StopFollowing;
        GameStateManager.GameEnded += RestartFromBeginning;
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }        
    }

    private void OnDisable()
    {
        GameStateManager.GameStarted -= StartFollowing;
        GameStateManager.GameEnded -= StopFollowing;
        GameStateManager.GameEnded -= RestartFromBeginning;
        if (pathCreator != null) pathCreator.pathUpdated -= OnPathChanged;
    }

    private void Start()
    {
        _activeTerrain = Terrain.activeTerrain;
    }

    void Update()
    {
        if (_isFollowing && pathCreator != null)
        {
            DistanceTravelled += Speed * Time.deltaTime;
            var XZPos = pathCreator.path.GetPointAtDistance(DistanceTravelled, endOfPathInstruction);
            var height = _activeTerrain.SampleHeight(XZPos) + _activeTerrain.transform.position.y + yOffset;
            transform.position = new Vector3(XZPos.x, height, XZPos.z);
            transform.rotation = pathCreator.path.GetRotationAtDistance(DistanceTravelled, endOfPathInstruction);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    private void OnPathChanged() {
        DistanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    private void StartFollowing() => _isFollowing = true;
    private void StopFollowing() => _isFollowing = false;

    public void RestartFromBeginning() => DistanceTravelled = 0;
    public override PresettableCategory GetCategory() => PresettableCategory.Locomotion;
}
