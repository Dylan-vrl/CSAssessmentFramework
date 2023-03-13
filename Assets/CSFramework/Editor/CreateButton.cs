using System;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class CreateButton
    {
        private const string CreateButtonClass = "create";
        
        public static VisualElement Draw(Action onClick)
        {
            var button = new Button(onClick);
            button.Add(new Label("Create"));
            button.AddToClassList(CreateButtonClass);
            return button;
        }
    }
}