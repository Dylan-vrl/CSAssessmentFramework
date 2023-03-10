using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using CSFramework.Core;

namespace CSFramework.Editor
{
    using static EditorHelper;
    
    /// <summary>
    /// Main CSFramework window.
    /// </summary>
    public class SetupWindow : EditorWindow
    {
        private const string WindowTitle = "CSFramework";

        [SerializeField] private StyleSheet styleSheet;

        private Dictionary<ExtensionCategory, VisualElement> _categoriesWindows;

        private ExtensionCategory _selectedCategory;

        [MenuItem("CSFramework/Setup")]
        private static void CreateMenu()
        {
            var window = GetWindow<SetupWindow>();
            window.titleContent = new GUIContent(WindowTitle);
        }

        private void CreateGUI()
        {
            RebuildView(AllCategories[0]);
        }

        private void RebuildView(ExtensionCategory category)
        {
            rootVisualElement.Clear();
            rootVisualElement.styleSheets.Add(styleSheet);

            rootVisualElement.Add(CategoryListElement.Draw(
                AllCategories,
                category,
                RebuildView)
            );
            rootVisualElement.Add(CategoryElement.Draw(category));
        }
        
    }
}