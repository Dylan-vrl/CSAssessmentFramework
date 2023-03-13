using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class GameObjectIconButtonLabelElement
    {
        private const string GameObjectIconLabelClass = "iconLabel";
        
        public static VisualElement Draw(
            string label,
            Action onClick = null)
        {
            var root = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            root.AddToClassList(GameObjectIconLabelClass);
            

            // Icon and name
            root.Add(GameObjectIconButtonElement.Draw(onClick));
            root.Add(new Label(label));

            return root;
        }
    }
}