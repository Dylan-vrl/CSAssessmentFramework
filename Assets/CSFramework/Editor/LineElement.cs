using System;
using UnityEngine.UIElements;

namespace CSFramework.Editor
{
    public static class LineElement
    {
        private const String LineClass = "line";
        
        public static VisualElement Draw()
        {
            var root = new Box();
            root.AddToClassList(LineClass);
            return root;
        }
    }
}