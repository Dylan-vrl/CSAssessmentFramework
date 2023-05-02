using CSFramework.Core;
using CSFramework.Extensions;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Vision/RestFramesPreset", fileName = "new RestFramesPreset")]
    public class RestFramesPreset: Preset<RestFrames>
    {
        // TODO replace with your own fields following this format
        [Header("Nose")]
        [SerializeField] public bool nose;
        [SerializeField] public GameObject nosePrefab;
        [Range(0, 1)][SerializeField] public float yPosition = .5f;
        [Range(0, 1)][SerializeField] public float zPosition = .5f;
        [Range(0f, 1f)][SerializeField] public float noseWidth = 1;
        [Range(0f, 1f)][SerializeField] public float noseFlatness = 1;

        [Header("Hat")]
        [SerializeField] public bool hat;
        [SerializeField] public GameObject hatPrefab;
    }
}