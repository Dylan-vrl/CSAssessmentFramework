using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;

namespace CSFramework.Extensions
{
    public class TerrainModifier : Extension<Terrain, TerrainModifierPreset>
    {
        public override PresettableCategory GetCategory() => Environment;

        [SerializeField] private Transform rotateWithTerrain;

        private int _tRes;

        private Terrain _terrain;
        public Terrain TargetTerrain => _terrain;

        private void OnEnable()
        {
            _terrain = Terrain.activeTerrain;
            _tRes = TargetTerrain.terrainData.heightmapResolution;
        }


        /// <summary>
        /// Applies the terrain modifications to the terrain, and rotates <see cref="rotateWithTerrain"/> to correspond.
        /// </summary>
        public void Apply()
        {
            var tMaxHeight = TargetTerrain.terrainData.heightmapScale.y;
            Transform transformT = TargetTerrain.transform;
            Vector3 position = transformT.position;
            position = new Vector3(position.x, -tMaxHeight/2, position.z);
            transformT.position = position;
            var arr = new float[_tRes, _tRes];
            // At each point of the terrain.
            for (var k = 0; k < _tRes; k+=1)
            {
                for (var l = 0; l < _tRes; l+=1)
                {
                    TerrainData terrainData = TargetTerrain.terrainData;
                    var xPos = k * terrainData.size.x / _tRes;
                    var zPos = l * terrainData.size.z / _tRes;

                    //Orienting terrain
                    float orientation;
                    if (Preset.AngleApplyToZNotX)
                    {
                        orientation = (zPos - terrainData.size.z / 2f) *Mathf.Sin(Preset.Angle * Mathf.Deg2Rad)/Mathf.Sin((90-Preset.Angle) * Mathf.Deg2Rad);
                    }
                    else
                    {
                        orientation = (xPos - terrainData.size.x / 2f) * Mathf.Sin(-Preset.Angle * Mathf.Deg2Rad)/Mathf.Sin((90-Preset.Angle) * Mathf.Deg2Rad);
                    }
                    
                    //applying the small scale modification
                    var h = (Preset.CurveApplyToZ ? Preset.SmallScaleCurve.Evaluate(xPos% (Preset.SizeOfCurve+1) / Preset.SizeOfCurve) : 1) 
                            * (Preset.CurveApplyToX ? Preset.SmallScaleCurve.Evaluate(zPos% (Preset.SizeOfCurve+1) / Preset.SizeOfCurve) : 1);
                    if (!Preset.CurveApplyToX && !Preset.CurveApplyToZ)
                    {
                        h = 0;
                    }

                    if (Preset.UsePerlinNoise)
                    {
                        // Scaling down effect near 0,0 (spawn)
                        var perlinBlendX = Mathf.Clamp01(Mathf.Abs(xPos - terrainData.size.x / 2f) / 20f);
                        var perlinBlendZ = Mathf.Clamp01(Mathf.Abs(zPos - terrainData.size.z / 2f) / 20f);
                        var perlinBlend = new Vector2(perlinBlendX, perlinBlendZ).magnitude / Mathf.Sqrt(2);

                        var xP = xPos / Preset.PerlinScale + Preset.PerlinSeed;
                        var zP = zPos / Preset.PerlinScale + Preset.PerlinSeed;
                        var sample = Mathf.Clamp01(Mathf.PerlinNoise(xP, zP));
                        h += (sample - 0.5f)*Preset.PerlinIntensity*perlinBlend;
                    }
                    
                    arr[k, l] = (h+orientation+tMaxHeight/2)/tMaxHeight;
                }
                    
            }
            TargetTerrain.terrainData.SetHeights(0,0,arr);
            
            if (rotateWithTerrain != null)
            {
                rotateWithTerrain.eulerAngles = Preset.AngleApplyToZNotX ? new Vector3(0,0,Preset.Angle) : new Vector3(Preset.Angle,0,0);
            }
        }
    }
}
