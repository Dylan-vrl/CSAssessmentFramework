using CSFramework.Core;
using CSFramework.Extensions;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Vision/DepthOfFieldPreset", fileName = "new DepthOfFieldPreset")]
    public class DepthOfFieldPreset: Preset<DepthOfField>
    {
        [field: Range(0.5f, 1.5f), SerializeField] public float BlurIntensity { get; private set; }= 1;
        [field: Header("Constant Blur")]
        [field: SerializeField] public bool ConstantBlur { get; private set; }

        [field: Range(0, 50), SerializeField] public float BlurStartDistance { get; private set; } = 10f;
        [field: Range(0, 50), SerializeField] public float BlurMaxDistance { get; private set; } = 20f;
        [field: Header("Dynamic Blur")]
        [field: SerializeField] public bool DynamicBlur { get; private set; }
        [field: Range(0, 50), SerializeField] public float DynamicBlurStartDistance { get; private set; } = 10f;
        [field: Range(0, 50), SerializeField] public float DynamicBlurMaxDistance { get; private set; } = 20f;
    	
    }
}