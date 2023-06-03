using System;
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

        private Dictionary<PresettableCategory, VisualElement> _categoriesWindows;

        private PresettableCategory _selectedCategory;
        private ScrollView _scrollView;

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

        private void OnHierarchyChange()
        {
            if (Application.isPlaying && rootVisualElement.childCount != 0) return;
            RebuildView(_selectedCategory);
        }

        private void RebuildView(PresettableCategory category)
        {
            rootVisualElement.Clear();
            rootVisualElement.styleSheets.Add(styleSheet);

            rootVisualElement.Add(CategoryListElement.Draw(
                AllCategories,
                category,
                RebuildView)
            );
            
            _scrollView ??= new ScrollView
            {
                viewDataKey = "category_scroll_view"
            };
            
            _scrollView.Clear();
            
            _scrollView.Add(CategoryElement.Draw(category, () => RebuildView(category)));
            rootVisualElement.Add(_scrollView);
            _selectedCategory = category;
        }
    }
}