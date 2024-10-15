using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Naux.CompressSettingsTool
{
    public class CompressSettings : EditorWindow
    {
        private string folderPath = "Assets/";
        private string[] spritesFound = new string[0];
        private CustomPlatformType cusPlatformType = CustomPlatformType.Default;
        private CustomTextureImporter cusTexImporter;
        private PlatformHandler platformHandler;


        [MenuItem("Tools/" + nameof(CompressSettings))] static void ShowWindow() => EditorWindow.GetWindow<CompressSettings>();
        private void OnEnable()
        {
            cusTexImporter = new CustomTextureImporter();
            platformHandler = new PlatformHandler();
            platformHandler.Initialized();
        }
        private void OnGUI()
        {
            DrawChooseFolderPanel();
            DrawTextureImporterPanel();
            DrawCompressSettingsPanel();
            DrawButtonAutoSetting();
        }


        private void DrawChooseFolderPanel()
        {
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
            if (GUILayout.Button("Browser", GUILayout.Width(100)))
            {
                var _selectedPath = EditorUtility.OpenFolderPanel("Select Folder", folderPath, "Assets");
                if (!string.IsNullOrWhiteSpace(_selectedPath))
                {
                    var _fullPath = new Uri(_selectedPath);
                    var _projectPath = new Uri(Application.dataPath);
                    folderPath = _projectPath.MakeRelativeUri(_fullPath).ToString().Replace('/', '\\');

                    if (!folderPath.StartsWith("Assets"))
                        folderPath = _selectedPath;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
        private void DrawTextureImporterPanel()
        {
            cusTexImporter.textureType = (TextureImporterType)EditorGUILayout.EnumPopup($"Texture Type", cusTexImporter.textureType);
            cusTexImporter.mipmapEnabled = EditorGUILayout.Toggle("Generate Mip Maps", cusTexImporter.mipmapEnabled);
            cusTexImporter.wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", cusTexImporter.wrapMode);
            cusTexImporter.filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", cusTexImporter.filterMode);
            EditorGUILayout.Space(20);
        }
        private void DrawCompressSettingsPanel()
        {
            cusPlatformType = (CustomPlatformType)GUILayout.Toolbar((int)cusPlatformType, Enum.GetNames(typeof(CustomPlatformType)));
            EditorGUILayout.Space(2);
            platformHandler.DrawCompressSettings(cusPlatformType);
            EditorGUILayout.Space(20);
        }
        private void DrawButtonAutoSetting()
        {
            if (GUILayout.Button("Auto Setting"))
            {
                spritesFound = FindAssetPathsWithPath(folderPath);
                for (int i = 0, _count = spritesFound.Length; i < _count; i++)
                {
                    TextureImporter _textureImporter = (TextureImporter)AssetImporter.GetAtPath(spritesFound[i]);
                    int _size = GetMaxSizeTexture(spritesFound[i]);
                    OptimizeSprite(_textureImporter, cusTexImporter, platformHandler, _size);
                }
                AssetDatabase.Refresh();
                Debug.Log("Auto Setting Completed!!");
            }
        }


        public string[] FindAssetPathsWithPath(string _folderPath)
        {
            var _encryptedPaths = AssetDatabase.FindAssets($"t:Texture2D", new[] { _folderPath });
            var _decryptPaths = new List<string>();

            for (int i = 0; i < _encryptedPaths.Length; i++)
            {
                var _decrypt = AssetDatabase.GUIDToAssetPath(_encryptedPaths[i]);
                var fileExtension = Path.GetExtension(_decrypt).ToLower();

                if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" ||
                    fileExtension == ".tga" || fileExtension == ".psd" || fileExtension == ".bmp")
                    _decryptPaths.Add(_decrypt);
            }

            return _decryptPaths.ToArray();
        }
        private void OptimizeSprite(TextureImporter texImporter, CustomTextureImporter cusTexImporter, PlatformHandler platformHandler, int maxSize)
        {
            if (texImporter != null)
            {
                texImporter.textureType = cusTexImporter.textureType;
                texImporter.mipmapEnabled = cusTexImporter.mipmapEnabled;
                texImporter.wrapMode = cusTexImporter.wrapMode;
                texImporter.filterMode = cusTexImporter.filterMode;
                //texImporter.SetTextureSettings();

                var _allPlatform = platformHandler.LstCusPlatSettings;
                for (int i = 0, _count = _allPlatform.Count; i < _count; i++)
                {
                    var _platform = _allPlatform[i].customPlatformType;

                    var _textureImporterPlatformSettings = _platform == CustomPlatformType.Default
                        ? texImporter.GetDefaultPlatformTextureSettings() : texImporter.GetPlatformTextureSettings($"{_platform}");
                    platformHandler.HandleMergeSettings(_platform, _textureImporterPlatformSettings, maxSize);

                    texImporter.SetPlatformTextureSettings(_textureImporterPlatformSettings);
                }

                texImporter.SaveAndReimport();
            }
        }
        private int GetMaxSizeTexture(string spritePath)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath);
            int width = texture.width;
            int height = texture.height;

            int maxSize = Mathf.Max(width, height);
            switch (maxSize)
            {
                case <= 64:
                    return 64;
                case <= 128:
                    return 128;
                case <= 256:
                    return 256;
                case <= 512:
                    return 512;
                case <= 1024:
                    return 1024;
                case <= 2048:
                    return 2048;
                case <= 4096:
                    return 4096;
                case <= 8192:
                    return 8192;
                default:
                    return 16384;
            }
        }
    }
}