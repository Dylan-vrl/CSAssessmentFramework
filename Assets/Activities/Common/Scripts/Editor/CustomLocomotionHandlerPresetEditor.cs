using System.Collections.Generic;
using CSFramework.Editor;
using CSFramework.Presets;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[CustomEditor(typeof(LocomotionHandlerPreset))]
public class CustomLocomotionHandlerPresetEditor: Editor
{
    private VisualElement root;
    private List<VisualElement> historyInspectors = new();
    
    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();
        RebuildGUI();
        
        var preset = (LocomotionHandlerPreset)serializedObject.targetObject;
        preset.OnListChange.AddListener(RebuildGUI);        
        
        return root;
    }

    private void RebuildGUI()
    {
        var leftProvidersProperty = serializedObject.FindProperty("leftActiveLocomotionProviders");
        var rightProvidersProperty = serializedObject.FindProperty("rightActiveLocomotionProviders");

        var leftProvidersPropertyField = new PropertyField(leftProvidersProperty);
        var rightProvidersPropertyField = new PropertyField(rightProvidersProperty);

        root.Add(leftProvidersPropertyField);
        root.Add(rightProvidersPropertyField);
        
        RemoveHistory();

        AddProviderPropertyInspector(leftProvidersProperty, "Left");
        AddProviderPropertyInspector(rightProvidersProperty,"Right");
    }

    private void RemoveHistory()
    {
        foreach (var toRemove in historyInspectors)
        {
            root.Remove(toRemove);
        }
        
        historyInspectors.Clear();
    }
    
    private void AddProviderPropertyInspector(SerializedProperty property, string labelPrefix)
    {
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

            historyInspectors.Add(inspectorTitle);
            historyInspectors.Add(inspector);
            historyInspectors.Add(line);

            root.Add(inspectorTitle);
            root.Add(inspector);
            root.Add(line);
        }
    }
}
