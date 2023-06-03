using CSFramework.Core;
using CSFramework.Extensions;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Vision/ColorManipulationPreset", fileName = "new ColorManipulationPreset")]
    public class ColorManipulationPreset: Preset<ColorManipulation>
    {
        // TODO replace with your own fields following this format
        [Header("Hue Manipulation")]
        [SerializeField] public bool hueManipulation;
        [Range(0, 1)][SerializeField] public float redDegradation = 1f;
        [Range(0, 1)][SerializeField] public float greenDegradation = 1f;
        [Range(0, 1)][SerializeField] public float blueDegradation = 1f;
        [Range(0, 1)][SerializeField] public float whiteDegradation = 1f;
        [Header("Contrast and Saturation")]
        [SerializeField] public bool contrastSaturation;
        [Range(-100f, 100f)][SerializeField] public float contrastStrength = 0f;
        [Range(-100f, 100f)][SerializeField] public float saturationStrength = 0f;
    }
}