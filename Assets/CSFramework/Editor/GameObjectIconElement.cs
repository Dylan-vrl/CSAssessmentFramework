using UnityEditor;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class GameObjectIconElement
    {
        public static VisualElement Draw()
        {
            var iconTexture = EditorGUIUtility.IconContent("GameObject Icon", "|Open in hierarchy").image;
            var image = new Image
            {
                image = iconTexture
            };
            return image;
        }
    }
}