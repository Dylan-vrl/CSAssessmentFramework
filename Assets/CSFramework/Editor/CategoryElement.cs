using UnityEngine.UIElements;

using CSFramework.Core;

namespace CSFramework.Editor
{
    using static EditorHelper;
    
    public static class CategoryElement
    {
        private const string RootClassList = "categoryRoot";

        public static VisualElement Draw(
            ExtensionCategory category)
        {
            var root = new VisualElement();
            root.AddToClassList(RootClassList);

            var extensionsByGameObject = ExtensionsByGameObjectForCategory(category);
            foreach (var tuple in extensionsByGameObject)
            {
                var gameObject = tuple.Key;
                var extensionsInfo = tuple.Value;

                root.Add(GameObjectIconLabelElement.Draw(gameObject));
                foreach (var extensionInfo in extensionsInfo)
                {
                    var extensionView = ExtensionElement.Draw(gameObject, extensionInfo.Item1);
                    if (extensionView.childCount != 0)
                        root.Add(extensionView);
                }
                
                root.Add(LineElement.Draw());
            }

            return root;
        }
    }
}