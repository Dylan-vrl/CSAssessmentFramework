using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using CSFramework.Core;

namespace CSFramework.Editor
{
    /// <summary>
    /// Custom editor for extensions.
    /// </summary>
    [CustomEditor(typeof(Extension<,>), true)]
    public class ExtensionEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var presetProperty = serializedObject.FindProperty("preset");
            var presetField = new PropertyField(presetProperty)
            {
                label = "" // To directly display the ObjectField
            };
            presetField.RegisterValueChangeCallback(RebuildFoldout);
            root.Add(presetField);

            return root;

            void RebuildFoldout(SerializedPropertyChangeEvent changeEvent)
            {
                // Remove previous preset's inspector
                if (root.childCount >= 2)
                    root.RemoveAt(1);

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

                root.Add(foldout);
            }
        }
    }
}
