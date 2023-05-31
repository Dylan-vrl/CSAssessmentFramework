using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public class PresetElement
    {
        private VisualElement _lastFoldout;   
        private VisualElement _presetField;
        private SerializedObject _serializedObject;   
        private VisualElement _root;

        public PresetElement(SerializedObject serializedObject, VisualElement root)
        {
            _serializedObject = serializedObject;
            _root = root;
        }
        
        public VisualElement Draw() {
            var presetProperty = _serializedObject.FindProperty("preset");
            var presetField = new PropertyField(presetProperty)
            {
                label = "" // To directly display the ObjectField
            };
            presetField.RegisterValueChangeCallback(RebuildFoldout);
            _presetField = presetField;
            return presetField;
        }
        
        private void RebuildFoldout(SerializedPropertyChangeEvent changeEvent)
        {
            var foldoutIndex = _root.IndexOf(_presetField) + 1;

            // Remove previous preset's inspector
            if (_lastFoldout != null)
            {
                var lastFoldoutIndex = _root.IndexOf(_lastFoldout);
                if (lastFoldoutIndex != -1)
                {
                    _root.RemoveAt(lastFoldoutIndex);
                }
            }

            var newProperty = changeEvent.changedProperty;

            // If no new preset, do not draw the foldout
            if (newProperty?.boxedValue == null)
                return;

            // Create the foldout for the new preset
            var foldout = new Foldout
            {
                value = false,
                // Used to keep it open when switching context
                viewDataKey = $"{_serializedObject.targetObject.name}{newProperty?.boxedValue}InspectorFoldout",
                text = "Modify preset"
            };
            
            foldout.AddToClassList("foldout");
            _lastFoldout = foldout;

            var inspector = new InspectorElement(newProperty.objectReferenceValue)
            {
                style =
                {
                    minWidth = 500
                }
            };
            
            foldout.Add(inspector);
            /*// Draw all internal fields of the preset
            SerializedObject targetObject = new SerializedObject(newProperty?.objectReferenceValue);
            SerializedProperty field = targetObject.GetIterator();
            field.NextVisible(true); // Skip script name

            while (field.NextVisible(false))
            {
                var fieldField = new PropertyField(field);
                fieldField.RegisterValueChangeCallback(_ => targetObject.ApplyModifiedProperties());
                fieldField.Bind(field.serializedObject);
                foldout.Add(fieldField);
            }*/

            if (foldoutIndex != -1)
            {
                _root.Insert(foldoutIndex, foldout);
            }
        }
    }
}