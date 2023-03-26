using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

using CSFramework.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CSFramework.Editor
{
    using static EditorHelper;
    
    public static class CategoryElement
    {
        private const string RootClass = "categoryRoot";
        private const string ExtensionsRootClass = "extensionsRoot";
        private const string Title2Class = "title2";
        private const string Title3Class = "title3";

        public static VisualElement Draw(
            PresettableCategory category,
            Action onUpdate
            )
        {
            var root = new VisualElement();
            root.AddToClassList(RootClass);

            root.Add(CreateAssetsElement.Draw(category));
            
            CreatePresettableElements(
                root, 
                onUpdate,
                "Non-Extensions",
                NonExtensionsByInSceneGameObjectForCategory(category),
                NotInSceneNonExtensionsForCategory(category),
                NonExtensionOnToggleChanged,
                NonExtensionGroupRule
                );
            CreatePresettableElements(
                root,
                onUpdate,
                "Extensions",
                ExtensionsByInSceneGameObjectForCategory(category),
                NotInSceneExtensionsForCategory(category),
                ExtensionOnToggleChanged,
                ExtensionGroupRule);
            
            return root;

            static Type NonExtensionGroupRule((Type, IPresettable) tuple) => tuple.Item1;
            static Type ExtensionGroupRule((Type, IExtension) tuple) => tuple.Item2.ExtendedType;

            static void NonExtensionOnToggleChanged(
                VisualElement root,
                Toggle toggle,
                GameObject gameObject,
                Type presettableType,
                ChangeEvent<bool> changeEvent)
            {
                if (changeEvent.newValue == false)
                {
                    if (!gameObject.TryGetComponent(presettableType, out var component)) return;

                    if (root.childCount >= 2)
                        root.RemoveAt(root.childCount - 1);

                    if (gameObject.GetComponents(typeof(IPresettable)).Length > 1)
                        Object.DestroyImmediate(component);
                    else
                    {
                        toggle.SetEnabled(false);
                        Object.DestroyImmediate(gameObject);
                    }
                }
                else
                {
                    if (gameObject == null) return;

                    gameObject.AddComponent(presettableType);
                    var inspector = new InspectorElement(gameObject.GetComponent(presettableType));
                    root.Add(inspector);
                }
            }
            static void ExtensionOnToggleChanged(
                VisualElement root,
                Toggle toggle,
                GameObject gameObject, 
                Type extensionType, 
                ChangeEvent<bool> changeEvent)
            {
                if (changeEvent.newValue == false)
                {
                    if (gameObject == null) return;
                    
                    if (!gameObject.TryGetComponent(extensionType, out var component)) return;

                    if (root.childCount >= 2)
                        root.RemoveAt(root.childCount - 1);
                    Object.DestroyImmediate(component);
                }
                else
                {
                    if (gameObject == null) return;

                    gameObject.AddComponent(extensionType);
                    var extensionInspector = new InspectorElement(gameObject.GetComponent(extensionType));
                    root.Add(extensionInspector);
                }
            }
        }

        private static void CreatePresettableElements<T> (
            VisualElement root, 
            Action onUpdate,
            String title,
            Dictionary<GameObject, IEnumerable<(Type, T)>> inSceneByGameObject,
            IEnumerable<(Type, T)> notInScene,
            Action<VisualElement, Toggle, GameObject, Type, ChangeEvent<bool>> onToggleChanged,
            Func<(Type, T), Type> groupRule
        )
            where T: IPresettable
        {
            if (notInScene.Any() || inSceneByGameObject.Any())
            {
                var titleLabel = new Label(title);
                titleLabel.AddToClassList(Title2Class);
                root.Add(titleLabel);
            }
            else
            {
                return;
            }

            var presetRoot = new VisualElement();
            presetRoot.AddToClassList(ExtensionsRootClass);
            
            if (inSceneByGameObject.Any())
            {
                var inSceneTitle = new Label("In Scene");
                inSceneTitle.AddToClassList(Title3Class);
                presetRoot.Add(inSceneTitle);

                var count = 0;
                foreach (var tuple in inSceneByGameObject)
                {
                    var gameObject = tuple.Key;
                    var infos = tuple.Value;

                    presetRoot.Add(GameObjectIconButtonLabelElement.Draw(gameObject.name, () => Selection.SetActiveObjectWithContext(gameObject, null)));
                    foreach (var info in infos)
                    {
                        void OnToggleChangedLocal(VisualElement root, Toggle toggle, ChangeEvent<bool> changeEvent) 
                            => onToggleChanged(root, toggle, gameObject, info.Item1, changeEvent);

                        var presetView = InScenePresettableElement.Draw(gameObject, info.Item1, OnToggleChangedLocal);
                        if (presetView.childCount != 0)
                            presetRoot.Add(presetView);
                    }

                    count += 1;

                    if (count != inSceneByGameObject.Count)
                        presetRoot.Add(LineElement.Draw());
                }
            }
            
            if (notInScene.Any())
            {
                var notInSceneTitle = new Label("Not In Scene");
                notInSceneTitle.AddToClassList(Title3Class);
                presetRoot.Add(notInSceneTitle);

                var notInSceneByType = notInScene.GroupBy(groupRule);
                
                foreach (var entry in notInSceneByType)
                {
                    var thisType = entry.Key;

                    var iconLabel = GameObjectIconButtonLabelElement.Draw(thisType.Name, () => { });
                    iconLabel.Add(CreateButton.Draw(() =>
                    {
                        CreateGameObjectWithComponent(thisType);
                        onUpdate();
                    }));
                    presetRoot.Add(iconLabel);

                    foreach (var tuple in entry)
                    {
                        var type = tuple.Item1;
                        
                        var presetView = NotInScenePresettableElement.Draw(type);
                        if (presetView.childCount != 0)
                            presetRoot.Add(presetView);
                    }
                }
            }

            root.Add(presetRoot);
        }

        private static void CreateGameObjectWithComponent(Type type)
        {
            new GameObject(
                name: type.Name, 
                components: type
            );
        }
    }
}