using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

using CSFramework.Core;

namespace CSFramework.Editor
{
    public static class EditorHelper
    {
        public static IEnumerable<Type> AllExtensionTypes
        {
            get
            {
                var iExtensionType = typeof(IExtension);
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(t => iExtensionType.IsAssignableFrom(t)
                                && t != iExtensionType
                                && t != typeof(Extension<,>));
            }
        }

        public static IEnumerable<IExtension> AllExtensions =>
            AllExtensionTypes.Select(t => (IExtension)FormatterServices.GetUninitializedObject(t));
        
        public static ExtensionCategory[] AllCategories =>
            (ExtensionCategory[])Enum.GetValues(typeof(ExtensionCategory));
        
        public static Dictionary<GameObject, IEnumerable<(Type, IExtension)>> ExtensionsByGameObjectForCategory(
            ExtensionCategory category)
        {
            Dictionary<GameObject, IEnumerable<(Type, IExtension)>> dict =
                new Dictionary<GameObject, IEnumerable<(Type, IExtension)>>();

            var gameObjectsByExtension = AllExtensionTypes.Zip(AllExtensions, (type, extension) => (type, extension))
                .Where(extensionInfo => extensionInfo.extension.Category == category)
                .Select(extensionInfo => (extensionInfo, extensionInfo.extension.ExtendedType))
                .Select(triple => (triple.extensionInfo, GetGameObjectsOfTypeInScene(triple.ExtendedType)));

            foreach (var triple in gameObjectsByExtension)
            {
                var gameObjects = triple.Item2;
                foreach (var gameObject in gameObjects)
                {
                    var prevList = dict.GetValueOrDefault(gameObject, new List<(Type, IExtension)>());
                    dict.Remove(gameObject);
                    dict.Add(gameObject, prevList.Append(triple.extensionInfo));
                }
            }

            return dict;
        }
        
        public static List<GameObject> GetGameObjectsOfTypeInScene(Type type)
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            var allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            if (allObjects == null)
            {
                Debug.LogError($"Error while fetching all the {nameof(GameObject)}s in the Scene.");
                return objectsInScene;
            }

            foreach (GameObject go in allObjects)
            {
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject)
                    && !(go.hideFlags == HideFlags.NotEditable
                         || go.hideFlags == HideFlags.HideAndDontSave)
                    && go.TryGetComponent(type, out _))
                    objectsInScene.Add(go);
            }

            return objectsInScene;
        }
    }
}