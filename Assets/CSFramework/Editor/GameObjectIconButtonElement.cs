using System;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class GameObjectIconButtonElement
    {
        private const string GameObjectIconButtonClass = "icon";

        public static VisualElement Draw(Action onClick = null)
        {
            var iconButton = new Button();
            iconButton.AddToClassList(GameObjectIconButtonClass);
            iconButton.Add(GameObjectIconElement.Draw());
            if(onClick != null)
                iconButton.clicked += onClick;

            return iconButton;
        }
    }
}