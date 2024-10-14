using FangsExtensionTool;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CompressSettings : EditorWindow
{
    private string folderPath = "Assets";
    private TextureImporterCustom textureImporterCustom;

    private BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
    private TextureImporterFormat textureImporterFormat = TextureImporterFormat.ASTC_5x5;



    private string[] spritesFound = new string[0];



    [MenuItem("Tools/" + nameof(CompressSettings))]
     static void ShowWindow() => EditorWindow.GetWindow<CompressSettings>();
    private void OnEnable()
    {
        textureImporterCustom = new TextureImporterCustom();
    }

    private void OnGUI()
    {
        CompressSettingsPanel();
    }

    private void CompressSettingsPanel()
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

        if (textureImporterCustom == null)
        {
            Debug.Log("Null");
            return;
        }
        textureImporterCustom.textureType = (TextureImporterType)EditorGUILayout.EnumPopup($"Texture Type", textureImporterCustom.textureType);
        textureImporterCustom.mipmapEnabled = EditorGUILayout.Toggle("Generate Mip Maps", textureImporterCustom.mipmapEnabled);
        textureImporterCustom.wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", textureImporterCustom.wrapMode);
        textureImporterCustom.filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", textureImporterCustom.filterMode);






        buildTargetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup($"{nameof(BuildTargetGroup)}", buildTargetGroup);
        textureImporterFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", textureImporterFormat);

        if (GUILayout.Button("Optimizeaaaaaaa"))
        {
            spritesFound = ToolFindAssets.FindAssetPathsWithPath(ResourceType.Texture2D, folderPath);

            //OptimizeSprite(spritesFound, buildTargetGroup, textureImportType, generateMipmaps, textureImporterFormat);
        }


    }
    private void OptimizeSprite(IReadOnlyList<string> decryptPaths, BuildTargetGroup buildTarget, TextureImporterType textureType, bool isMipMap, TextureImporterFormat format)
    {
        for (int i = 0; i < decryptPaths.Count; i++)
        {




            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(decryptPaths[i]);
            if (textureImporter != null)
            {
                //setup defaul
                textureImporter.textureType = textureType;
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
                textureImporter.mipmapEnabled = isMipMap;

                //compress
                var textureImporterPlatformSettings = textureImporter.GetPlatformTextureSettings("a");

                textureImporterPlatformSettings.overridden = true; // android and ios?
                textureImporterPlatformSettings.maxTextureSize = MaxSizeTexture(textureImporter, decryptPaths[i]);//its too long?
                textureImporterPlatformSettings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell; //now it same on inspector, i think
                textureImporterPlatformSettings.format = format;

                //textureImporterPlatformSettings.textureCompression = TextureImporterCompression.Uncompressed; //pc but its another name?
                //textureImporterPlatformSettings.crunchedCompression = false; //pc
                //textureImporterPlatformSettings.compressionQuality = (int)TextureCompressionQuality.Normal; //ios
                //textureImporterPlatformSettings.androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings; //ios
                //textureImporter.SetPlatformTextureSettings(textureImporterPlatformSettings); //may be notuse
                textureImporter.SaveAndReimport();
            }
        }
    }

    private int MaxSizeTexture(TextureImporter textureImporter, string spritePath)
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

public class TextureImporterCustom
{
    public TextureImporterType textureType = TextureImporterType.Sprite;
    public bool mipmapEnabled = false;
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    public FilterMode filterMode = FilterMode.Bilinear;
}
