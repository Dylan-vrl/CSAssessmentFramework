using CSFramework.Presets;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[CustomEditor(typeof(LocomotionHandlerPreset))]
public class LocomotionHandlerPresetEditor: Editor
{
    private LocomotionHandlerPreset preset;
    private VisualElement root;
    private PropertyField leftProvidersPropertyField;
    private PropertyField rightProvidersPropertyField;
    private PropertyField leftControllerPropertyField;
    private PropertyField rightControllerPropertyField;
    private SerializedProperty leftProvidersProperty;
    private SerializedProperty rightProvidersProperty;

    private void OnEnable()
    {
        preset = (LocomotionHandlerPreset)serializedObject.targetObject;
        preset.OnListChange.AddListener(RebuildGUI);
    }

    private void OnDisable()
    {
        preset.OnListChange.RemoveListener(RebuildGUI);
    }

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();
        var leftControllerProperty = serializedObject.FindProperty("leftControllerPrefab");
        var rightControllerProperty = serializedObject.FindProperty("rightControllerPrefab");

        leftControllerPropertyField = new PropertyField(leftControllerProperty);
        rightControllerPropertyField = new PropertyField(rightControllerProperty);
        root.Add(leftControllerPropertyField);
        root.Add(rightControllerPropertyField);
        
        leftProvidersProperty = serializedObject.FindProperty("leftActiveLocomotionProviders");
        rightProvidersProperty = serializedObject.FindProperty("rightActiveLocomotionProviders");

        leftProvidersPropertyField = new PropertyField(leftProvidersProperty);
        rightProvidersPropertyField = new PropertyField(rightProvidersProperty);
        
        root.Add(leftProvidersPropertyField);
        root.Add(rightProvidersPropertyField);

        RebuildGUI();
        
        return root;
    }
    
    private void RebuildGUI()
    {
        RemoveHistory();

        AddProviderPropertyInspector(leftProvidersProperty, "Left");
        AddProviderPropertyInspector(rightProvidersProperty,"Right");
    }

    private void RemoveHistory()
    {
        if (root == null) return;
        root.Clear();

        root.Add(leftControllerPropertyField);
        root.Add(rightControllerPropertyField);
        root.Add(leftProvidersPropertyField);
        root.Add(rightProvidersPropertyField);
        root.Bind(serializedObject);
    }
    
    private void AddProviderPropertyInspector(SerializedProperty property, string labelPrefix)
    {
        if (root == null) return;
        
        for (var i = 0; i < property.arraySize; i++)
        {
            var provider = (LocomotionProvider)property.GetArrayElementAtIndex(i).boxedValue;
            if (provider == null) break;
            
            var inspectorTitle = new Label($"{labelPrefix}: {provider.name}")
            {
                style =
                {
                    fontSize = 16,
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                    paddingBottom = 8,
                    paddingTop = 16
                }
            };
            var inspector = new InspectorElement(provider)
            {
                style =
                {
                    paddingBottom = 16
                }
            };
            var line = new Box
            {
                style =
                {
                    height = 4,
                }
            };

            root.Add(inspectorTitle);
            root.Add(inspector);
            root.Add(line);
            root.Bind(serializedObject);
        }
    }
}
