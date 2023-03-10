using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

using CSFramework.Core;

namespace CSFramework.Editor
{
    /// <summary>
    /// Defines the visual element for the category (horizontally scrollable) list of the
    /// main window.
    /// </summary>
    public static class CategoryListElement
    {
        private const string CategoryListClass = "categoryList";
        private const string CategoryButtonClass = "category";
        private const string CategoryButtonSelectedClass = "selected";

        /// <summary>
        /// Returns the category list view (ScrollView) according to the given selected category.
        /// </summary>
        /// <param name="categories"> List of all categories </param>
        /// <param name="selectedCategory"> The category which is selected </param>
        /// <param name="onClick"> What to execute on click of one button </param>
        public static VisualElement Draw(
            IEnumerable<ExtensionCategory> categories, 
            ExtensionCategory selectedCategory,
            Action<ExtensionCategory> onClick)
        {
            var root = new VisualElement();
            root.AddToClassList(CategoryListClass);

            var scrollView = new ScrollView
            {
                viewDataKey = "CategoryListScrollView",
                mode = ScrollViewMode.Horizontal
            };
            scrollView.AddToClassList(CategoryListClass);

            foreach (var category in categories)
            {
                var button = new Button
                {
                    text = category.ToString()
                };
                button.clickable.clicked += () => onClick(category);
                button.AddToClassList(CategoryButtonClass);
                if (selectedCategory == category)
                    button.AddToClassList(CategoryButtonSelectedClass);
        
                scrollView.Add(button);
            }       
    
            root.Add(scrollView);
    
            return root;
        }
    }
}