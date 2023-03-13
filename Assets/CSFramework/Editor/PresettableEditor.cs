using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
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
        private VisualElement _presetField;
        private VisualElement _lastFoldout;   

        public override VisualElement CreateInspectorGUI()
        {

            var root = new VisualElement();
            root.Add(PresetField());
            root.Add(new IMGUIContainer(OnInspectorGUI));

            return root;

            VisualElement PresetField()
            {
                var presetProperty = serializedObject.FindProperty("preset");
                var presetField = new PropertyField(presetProperty)
                {
                    label = "" // To directly display the ObjectField
                };
                presetField.RegisterValueChangeCallback(RebuildFoldout);
                _presetField = presetField;
                return presetField;
            }
            
            void RebuildFoldout(SerializedPropertyChangeEvent changeEvent)
            {
                // Remove previous preset's inspector
                if(_lastFoldout != null
                   && root.Children().Contains(_lastFoldout)) 
                    root.Remove(_lastFoldout);
                
                var newProperty = changeEvent.changedProperty;

                // If no new preset, do not draw the foldout
                if (newProperty?.boxedValue == null)
                    return;

                // Create the foldout for the new preset
                var foldout = new Foldout
                {
                    value = false,
                    // Used to keep it open when switching context
                    viewDataKey = $"{serializedObject.targetObject.name}{newProperty?.boxedValue}InspectorFoldout",
                    text = "Modify preset"
                };
                _lastFoldout = foldout;

                // Draw all internal fields of the preset
                SerializedObject targetObject = new SerializedObject(newProperty?.objectReferenceValue);
                SerializedProperty field = targetObject.GetIterator();
                field.NextVisible(true); // Skip script name

                while (field.NextVisible(false))
                {
                    var fieldField = new PropertyField(field);
                    fieldField.RegisterValueChangeCallback(_ => targetObject.ApplyModifiedProperties());
                    fieldField.Bind(field.serializedObject);
                    foldout.Add(fieldField);
                }

                root.Clear();
                root.Add(_presetField);
                root.Add(foldout);
                root.Add(new IMGUIContainer(OnInspectorGUI));
            }
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
