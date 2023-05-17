using CSFramework.Editor;
using CSFramework.Extensions;
using UnityEditor;
using UnityEngine;

namespace Options
{
    [CustomEditor(typeof(TerrainModifier))]
    public class TerrainModifierEditor : PresettableEditor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var myTerrainModifier = target as TerrainModifier;
            
            if (GUILayout.Button("Apply"))
            {
                if (myTerrainModifier != null)
                {
                    Undo.RegisterCompleteObjectUndo(myTerrainModifier.TargetTerrain.terrainData, "Apply terrain Changes");
                    if (myTerrainModifier != null) myTerrainModifier.Apply();
                }
            }
        }
    }
}