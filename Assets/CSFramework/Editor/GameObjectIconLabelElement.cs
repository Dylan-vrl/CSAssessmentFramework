using UnityEngine;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class GameObjectIconLabelElement
    {
        private const string GameObjectIconLabelClass = ".iconLabel";
        
        public static VisualElement Draw(
            GameObject gameObject)
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
            root.Add(GameObjectIconButtonElement.Draw(gameObject));
            root.Add(new Label(gameObject.name));

            return root;
        }
    }
}