using CSFramework.Editor;
using CSFramework.Presettables;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(CustomTeleportationProvider))]
public class CustomTeleportationProviderEditor: Editor
{
    public override VisualElement CreateInspectorGUI()
    {

        var root = new VisualElement();
        root.Add(new PresetElement(serializedObject, root).Draw());
        root.Add(new IMGUIContainer(OnInspectorGUI));

        return root;
    }
        
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        string[] toExclude = 
        {
            "preset",
            "m_Script",
            "m_DelayTime"
        };
        
        DrawPropertiesExcluding(serializedObject, toExclude);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
