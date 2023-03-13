using System;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class NotInScenePresettableElement
    {
        private const string ExtensionClass = "extension";

        public static VisualElement Draw(Type extensionType)
        {
            var root = new Box();
            root.AddToClassList(ExtensionClass);
            root.Add(new Label(extensionType.Name));
            return root;
        }
    }
}