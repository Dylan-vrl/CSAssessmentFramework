using CSFramework.Editor;
using CSFramework.Presettables;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(ExtendedDynamicMoveProvider))]
public class ExtendedDynamicMoveProviderEditor: Editor
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
            "m_LeftHandMovementDirection",
            "m_RightHandMovementDirection",
            "m_MoveSpeed",
            "m_EnableStrafe"
        };
        
        DrawPropertiesExcluding(serializedObject, toExclude);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
