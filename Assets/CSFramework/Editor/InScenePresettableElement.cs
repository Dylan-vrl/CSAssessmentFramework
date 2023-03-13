using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    using Object = UnityEngine.Object;

    public static class InScenePresettableElement
    {
        private const string ExtensionClass = "extension";

        public static VisualElement Draw(
            GameObject gameObject, 
            Type extensionType,
            Action<VisualElement, Toggle, ChangeEvent<bool>> onToggleChanged)
        {
            var root = new Box();
            root.AddToClassList(ExtensionClass);
            
            try
            {
                var toggle = new Toggle
                {
                    value = gameObject.GetComponent(extensionType) != null
                };
                root.Add(toggle);
                root.Add(new Label(extensionType.Name));

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