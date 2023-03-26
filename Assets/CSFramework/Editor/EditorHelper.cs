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
        /// <summary>
        /// Returns all subtypes of the given <paramref name="baseType"/> excluding
        /// interfaces and abstract classes.
        /// </summary>
        public static IEnumerable<Type> AllMonoBehaviourSubtypesFor(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => 
                    t.IsMonoBehaviourSubtypeOf(baseType));
        }

        private static bool IsMonoBehaviourSubtypeOf(this Type type, Type baseType)
        {
            return baseType.IsAssignableFrom(type) 
                   && type != baseType
                   && !type.IsAbstract
                   && !type.IsInterface
                   && typeof(MonoBehaviour).IsAssignableFrom(type);
        }

        public static IEnumerable<Type> AllExtensionSubtypes =>
            AllMonoBehaviourSubtypesFor(typeof(IExtension));

        public static IEnumerable<IExtension> AllExtensions =>
            AllExtensionSubtypes
                .Select(t => (IExtension)FormatterServices.GetUninitializedObject(t));

        public static IEnumerable<Type> AllNonExtensionsSubtypes =>
            AllMonoBehaviourSubtypesFor(typeof(IPresettable))
                .Where(t => !t.IsMonoBehaviourSubtypeOf(typeof(IExtension)));

        public static IEnumerable<IPresettable> AllNonExtensions =>
            AllNonExtensionsSubtypes
                .Select(t => (IPresettable)FormatterServices.GetUninitializedObject(t));
        
        public static PresettableCategory[] AllCategories =>
            (PresettableCategory[])Enum.GetValues(typeof(PresettableCategory));

        public static Dictionary<GameObject, IEnumerable<(Type, IExtension)>> ExtensionsByInSceneGameObjectForCategory(
            PresettableCategory category)
        {
            Dictionary<GameObject, IEnumerable<(Type, IExtension)>> dict =
                new Dictionary<GameObject, IEnumerable<(Type, IExtension)>>();

            var gameObjectsByExtension = AllExtensionSubtypes.Zip(AllExtensions, (type, extension) => (type, extension))
                .Where(extensionInfo => extensionInfo.extension.GetCategory() == category)
                .Select(extensionInfo => (extensionInfo, extensionInfo.extension.ExtendedType))
                .Select(triple => (triple.extensionInfo, GetGameObjectsWithComponentInScene(triple.ExtendedType)));

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

        public static IEnumerable<(Type, IExtension)> NotInSceneExtensionsForCategory(PresettableCategory category)
        {
            return AllExtensionSubtypes.Zip(AllExtensions, (type, extension) => (type, extension))
                .Where(extensionInfo => extensionInfo.extension.GetCategory() == category)
                .Where(extensionInfo =>
                    !GetGameObjectsWithComponentInScene(extensionInfo.extension.ExtendedType).Any());
        }

        public static Dictionary<GameObject, IEnumerable<(Type, IPresettable)>> NonExtensionsByInSceneGameObjectForCategory(
            PresettableCategory category)
        {
            Dictionary<GameObject, IEnumerable<(Type, IPresettable)>> dict =
                new Dictionary<GameObject, IEnumerable<(Type, IPresettable)>>();

            var gameObjectsByExtension = AllNonExtensionsSubtypes.Zip(AllNonExtensions, (type, nonExt) => (type, nonExt))
                .Where(nonExtInfo => nonExtInfo.nonExt.GetCategory() == category)
                .Select(nonExtInfo => (nonExtInfo, GetGameObjectsWithComponentInScene(nonExtInfo.type)));

            foreach (var triple in gameObjectsByExtension)
            {
                var gameObjects = triple.Item2;
                foreach (var gameObject in gameObjects)
                {
                    var prevList = dict.GetValueOrDefault(gameObject, new List<(Type, IPresettable)>());
                    dict.Remove(gameObject);
                    dict.Add(gameObject, prevList.Append(triple.nonExtInfo));
                }
            }

            return dict;
        }
        
        public static IEnumerable<(Type, IPresettable)> NotInSceneNonExtensionsForCategory(PresettableCategory category)
        {
            return AllNonExtensionsSubtypes.Zip(AllNonExtensions, (type, nonExt) => (type, nonExt))
                .Where(nonExtInfo => nonExtInfo.nonExt.GetCategory() == category)
                .Where(nonExtInfo =>
                    !GetGameObjectsWithComponentInScene(nonExtInfo.type).Any());
        }

        public static List<GameObject> GetGameObjectsWithComponentInScene(Type type)
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
                if (go.IsInScene()
                    && go.TryGetComponent(type, out _))
                {
                    objectsInScene.Add(go);
                }
            }

            return objectsInScene;
        }

        public static bool IsInScene(this GameObject gameObject)
        {
            return !EditorUtility.IsPersistent(gameObject.transform.root.gameObject) 
                   && !(gameObject.hideFlags == HideFlags.NotEditable 
                        || gameObject.hideFlags == HideFlags.HideAndDontSave);
        }
    }
}