using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class InScenePresettableElement
    {
        private const string ExtensionClass = "extension";
        private const string ExtensionLabelClass = "extensionLabel";

        public static VisualElement Draw(
            GameObject gameObject, 
            Type extensionType,
            Action<VisualElement, Toggle, ChangeEvent<bool>> onToggleChanged)
        {
            var root = new Box();
            root.AddToClassList(ExtensionClass);
            var extensionLabel = new Box();
            extensionLabel.AddToClassList(ExtensionLabelClass);
            
            try
            {
                var toggle = new Toggle
                {
                    value = gameObject.GetComponent(extensionType) != null
                };
                extensionLabel.Add(toggle);
                extensionLabel.Add(new Label(extensionType.Name));

                root.Add(extensionLabel);
                
                if (toggle.value)
                {
                    var extensionInspector = new InspectorElement(gameObject.GetComponent(extensionType));
                    root.Add(extensionInspector);
                }

                toggle.RegisterValueChangedCallback(e => onToggleChanged(root, toggle, e));
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning($"No preset found for Presettable {extensionType.Name}");
            }

            return root;
        }
    }
}