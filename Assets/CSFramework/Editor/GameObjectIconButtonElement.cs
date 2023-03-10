using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class GameObjectIconButtonElement
    {
        private const string GameObjectIconButtonClass = "icon";

        public static VisualElement Draw(
            GameObject gameObject)
        {
            var iconButton = new Button();
            iconButton.AddToClassList(GameObjectIconButtonClass);
            var iconTexture = EditorGUIUtility.IconContent("GameObject Icon", "|Open in hierarchy").image;
            var icon = new Image
            {
                image = iconTexture
            };
            iconButton.Add(icon);
            iconButton.clicked += () => Selection.SetActiveObjectWithContext(gameObject, null);

            return iconButton;
        }
    }
}