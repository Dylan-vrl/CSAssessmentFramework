using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    using Object = UnityEngine.Object;

    public static class ExtensionElement
    {
        private const string ExtensionClass = "extension";

        public static VisualElement Draw(
            GameObject gameObject, 
            Type extensionType
            )
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

                toggle.RegisterValueChangedCallback(OnToggleChanged);
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning($"No preset found for Extension {extensionType.Name}");
            }

            return root;

            void OnToggleChanged(ChangeEvent<bool> changeEvent)
            {
                if (changeEvent.newValue == false)
                {
                    if (!gameObject.TryGetComponent(extensionType, out var component)) return;

                    if (root.childCount >= 2)
                        root.RemoveAt(root.childCount - 1);
                    Object.DestroyImmediate(component);
                }
                else
                {
                    gameObject.AddComponent(extensionType);
                    var extensionInspector = new InspectorElement(gameObject.GetComponent(extensionType));
                    root.Add(extensionInspector);
                }
            }
        }
    }
}