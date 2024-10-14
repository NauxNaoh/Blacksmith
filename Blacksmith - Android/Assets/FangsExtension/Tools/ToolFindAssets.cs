using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FangsExtensionTool
{
    public class ToolFindAssets
    {
        public static string[] FindAssetPathsWithPath(ResourceType resourceType, string _folderPath)
        {
            var encryptedPaths = AssetDatabase.FindAssets($"t:{resourceType}", new[] {_folderPath});
            var decryptPaths = new string[encryptedPaths.Length];

            for (int i = 0; i < encryptedPaths.Length; i++)
            {
                string decryptPath = AssetDatabase.GUIDToAssetPath(encryptedPaths[i]);
                decryptPaths[i] = decryptPath;
            }

            return decryptPaths;
        }


        public static (IEnumerable<string>, List<GameObject>) FindAssetPathsInScene(ResourceType resourceType)
        {
            string[] paths = null;
            var lstGobj = new List<GameObject>();
            (string[], List<GameObject>) found;
            switch (resourceType)
            {
                case ResourceType.Texture2D:
                    found = GetTexture2DsPathsOnScene();
                    paths = found.Item1;
                    lstGobj = new List<GameObject>(found.Item2);
                    break;
                case ResourceType.Prefab:
                    found = GetPrefabsPathsOnScene();
                    paths = found.Item1;
                    lstGobj = new List<GameObject>(found.Item2);
                    break;
                case ResourceType.Material:
                    found = GetMaterialPathsOnScene();
                    paths = found.Item1;
                    lstGobj = new List<GameObject>(found.Item2);
                    break;
                default:
                    Debug.LogError("=====Type not support======");
                    break;
            }

            if (paths != null)
                Array.Sort(paths);
            else
                Debug.LogError("Path found is null");

            return (paths, lstGobj);
        }

        private static (string[], List<GameObject>) GetTexture2DsPathsOnScene()
        {
            var resourcePaths = new List<string>();
            var lstObjectRef = new List<GameObject>();

            // Lấy tất cả các game object trên scene
            var allGameObjects = Object.FindObjectsOfType<GameObject>();

            foreach (var go in allGameObjects)
            {
                // Kiểm tra các Component chứa Texture2D
                var components = go.GetComponents<Component>();

                foreach (var comp in components)
                {
                    if (comp is SpriteRenderer)
                    {
                        // Nếu là SpriteRenderer, kiểm tra sprite có Texture2D không
                        var spriteRenderer = (SpriteRenderer) comp;
                        if (spriteRenderer.sprite == null || spriteRenderer.sprite.texture == null) continue;

                        // Kiểm tra xem Texture2D có nằm trong thư mục "Assets" không
                        string texturePath = AssetDatabase.GetAssetPath(spriteRenderer.sprite.texture);
                        if (!texturePath.StartsWith("Assets/")) continue;

                        resourcePaths.Add(texturePath);
                        lstObjectRef.Add(go);
                    }
                    else if (comp is Image image)
                    {
                        // Nếu là Image trong UI, kiểm tra sprite có Texture2D không
                        if (image.sprite == null || image.sprite.texture == null) continue;

                        // Kiểm tra xem Texture2D có nằm trong thư mục "Assets" không
                        string texturePath = AssetDatabase.GetAssetPath(image.sprite.texture);
                        if (!texturePath.StartsWith("Assets/")) continue;

                        resourcePaths.Add(texturePath);
                        lstObjectRef.Add(go);
                    }
                }

                // Kiểm tra các Material của GameObject
                var allRenderer = go.GetComponentsInChildren<Renderer>(true);
                foreach (var renderer in allRenderer)
                {
                    var materials = renderer.sharedMaterials;
                    foreach (var material in materials)
                    {
                        if (material == null) continue;
                        var shader = material.shader;
                        if (shader == null) continue;

                        int propertyCount = ShaderUtil.GetPropertyCount(shader);
                        for (int i = 0; i < propertyCount; i++)
                        {
                            if (ShaderUtil.GetPropertyType(shader, i) != ShaderUtil.ShaderPropertyType.TexEnv) continue;

                            string textureName = ShaderUtil.GetPropertyName(shader, i);
                            var texture = material.GetTexture(textureName);
                            if (texture == null || !(texture is Texture2D)) continue;

                            string texturePath = AssetDatabase.GetAssetPath(texture);
                            if (!texturePath.StartsWith("Assets/")) continue;

                            resourcePaths.Add(texturePath);
                            lstObjectRef.Add(go);
                        }
                    }
                }
                
//                // Kiểm tra các script có trường GameObject
//                var scriptsWithGameObject = go.GetComponents<MonoBehaviour>();
//                foreach (var script in scriptsWithGameObject)
//                {
//                    var gameObjectFields = script.GetType()
//                        .GetFields(System.Reflection.BindingFlags.Public
//                                   | System.Reflection.BindingFlags.NonPublic
//                                   | System.Reflection.BindingFlags.Instance)
//                        .Where(field => field.FieldType == typeof(Sprite) && field.GetValue(script) != null);
//
//                    foreach (var field in gameObjectFields)
//                    {
//                        var referencedPrefab = field.GetValue(script) as Sprite;
//                        if (referencedPrefab == null) continue;
//
//                        string referencedPrefabPath = AssetDatabase.GetAssetPath(referencedPrefab);
//                        if (string.IsNullOrEmpty(referencedPrefabPath)
//                            || !referencedPrefabPath.StartsWith("Assets/")) continue;
//
//                        resourcePaths.Add(referencedPrefabPath);
//                        lstObjectRef.Add(go);
//                    }
//                }
            }

            return (resourcePaths.ToArray(), lstObjectRef);
        }

        private static (string[], List<GameObject>) GetPrefabsPathsOnScene()
        {
            var resourcePaths = new List<string>();
            var lstObjectRef = new List<GameObject>();

            // Lấy tất cả các game object trên scene
            var allGameObjects = Object.FindObjectsOfType<GameObject>();

            foreach (var go in allGameObjects)
            {
                // Kiểm tra nếu GameObject là Prefab và nếu nó có đường dẫn trong "Assets"
                if (!PrefabUtility.IsPartOfPrefabInstance(go)) continue;

                var rootPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go);
                if (rootPrefab == null) continue;

                string prefabPath = AssetDatabase.GetAssetPath(rootPrefab);
                if (!prefabPath.StartsWith("Assets/")) continue;

                resourcePaths.Add(prefabPath);
                lstObjectRef.Add(go);
            }

            foreach (var go in allGameObjects)
            {
                // Kiểm tra các script có trường GameObject
                var scriptsWithGameObject = go.GetComponents<MonoBehaviour>();
                foreach (var script in scriptsWithGameObject)
                {
                    var gameObjectFields = script.GetType()
                        .GetFields(System.Reflection.BindingFlags.Public
                                   | System.Reflection.BindingFlags.NonPublic
                                   | System.Reflection.BindingFlags.Instance)
                        .Where(field => field.FieldType == typeof(GameObject) && field.GetValue(script) != null);

                    foreach (var field in gameObjectFields)
                    {
                        var referencedPrefab = field.GetValue(script) as GameObject;
                        if (referencedPrefab == null) continue;

                        string referencedPrefabPath = AssetDatabase.GetAssetPath(referencedPrefab);
                        if (string.IsNullOrEmpty(referencedPrefabPath)
                            || !referencedPrefabPath.StartsWith("Assets/")) continue;

                        resourcePaths.Add(referencedPrefabPath);
                        lstObjectRef.Add(go);
                    }
                }
            }

            return (resourcePaths.ToArray(), lstObjectRef);
        }

        private static (string[], List<GameObject>) GetMaterialPathsOnScene()
        {
            var resourcePaths = new List<string>();
            var lstObjectRef = new List<GameObject>();
            // Lấy tất cả các game object trên scene
            var allGameObjects = Object.FindObjectsOfType<GameObject>();

            foreach (var go in allGameObjects)
            {
                // Kiểm tra các Material của Renderer trong GameObject và các Renderer con
                var allRenderer = go.GetComponents<Renderer>();
                foreach (var renderer in allRenderer)
                {
                    var materials = renderer.sharedMaterials;
                    foreach (var material in materials)
                    {
                        if (material == null || !EditorUtility.IsPersistent(material)) continue;

                        // Lấy đường dẫn của vật liệu và thêm vào danh sách
                        string materialPath = AssetDatabase.GetAssetPath(material);
                        if (string.IsNullOrEmpty(materialPath) || !materialPath.StartsWith("Assets/")) continue;

                        resourcePaths.Add(materialPath);
                        lstObjectRef.Add(go);
                    }
                }
            }

            return (resourcePaths.ToArray(), lstObjectRef);
        }
    }
}
#endif