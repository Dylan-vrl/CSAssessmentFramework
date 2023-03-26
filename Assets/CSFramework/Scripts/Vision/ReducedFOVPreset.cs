using CSFramework.Core;
using CSFramework.Extensions;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Vision/ReducedFOVPreset", fileName = "new ReducedFOVPreset")]
    public class ReducedFOVPreset: Preset<ReducedFOV>
    {
        [field: Header("Constant FOV")]
        [field: SerializeField] public bool ConstantFOV { get; private set; }
        [field: Range(0, 1), SerializeField] public float Intensity { get; private set; }
        [field: Range(0.01f, 1), SerializeField] public float Smoothness { get; private set; } = 0.2f;
        [field: Header("Dynamic FOV")]
        [field: SerializeField] public bool DynamicFOV { get; private set; }
        [field: SerializeField] public float TransitionSpeed { get; private set; } = 2f;
        [field: Range(0, 1), SerializeField] public float DynamicIntensity { get; private set; }
        [field: Range(0.01f, 1), SerializeField] public float DynamicSmoothness { get; private set; } = 0.2f;
    	
    }
}