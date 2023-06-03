using UnityEngine;
using CSFramework.Core;
using CSFramework.Extensions;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Environment/TerrainModifierPreset", fileName = "new TerrainModifierPreset")]
    public class TerrainModifierPreset: Preset<TerrainModifier>
    {
        [field: Header("Repeating Terrain Shape")]
        [field: SerializeField] public float SizeOfCurve { get; private set; } = 5f;
        [field: SerializeField] public bool CurveApplyToX { get; private set; } = true;
        [field: SerializeField] public bool CurveApplyToZ { get; private set; } = true;
        [field: SerializeField] public AnimationCurve SmallScaleCurve { get; private set; }
        [field: Header("Perlin Noise")]
        [field: SerializeField] public bool UsePerlinNoise{  get; private set; }
        [field: SerializeField, Range(1, 100)] public int PerlinSeed { get; private set; }
        [field: SerializeField, Range(5, 60)] 
        public int PerlinScale { get; private set; } = 10;
        [field: SerializeField, Range(0,30)] 
        public int PerlinIntensity { get; private set; } = 10;
        [field: Header("Terrain Orientation")]
        [field: SerializeField, Range(-20,20)] 
        public int Angle { get; private set; }
        [field: SerializeField] public bool AngleApplyToZNotX { get; private set; }
    }
}