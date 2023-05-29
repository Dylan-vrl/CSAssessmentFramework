using UnityEditor;
using UnityEngine.UIElements;
using CSFramework.Core;

namespace CSFramework.Editor
{
    /// <summary>
    /// Custom editor for extensions.
    /// </summary>
    [CustomEditor(typeof(PresettableMonoBehaviour<>), true)]
    public class PresettableEditor : UnityEditor.Editor
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
            DrawPropertiesExcluding(serializedObject, "preset", "m_Script");
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}
