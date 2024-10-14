#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FangsExtensionTool
{
    public class ToolOptimizerTexture2D
    {
        public static void OptimizeSprite(BuildTargetGroup buildTarget, IReadOnlyList<string> decryptPaths, TextureImporterType textureType, bool isMipMap, bool alphaTransparency, TextureImporterFormat format)
        {
            for (int i = 0; i < decryptPaths.Count; i++)
            {
                TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(decryptPaths[i]);
                if (textureImporter != null)
                {
                    textureImporter.textureType = textureType;
                    textureImporter.textureCompression = TextureImporterCompression.Compressed;
                    textureImporter.mipmapEnabled = isMipMap;
                    textureImporter.alphaIsTransparency = alphaTransparency;
                    //"Standalone", "iPhone" in 2021.f
                    //or "iOS" in 2020.f and below, "Android", "WebGL", "Windows Store Apps", "PS4", "XboxOne", "Nintendo Switch" and "tvOS".
                    textureImporter.SetPlatformTextureSettings($"{buildTarget}",
                        MaxSizeTexture(textureImporter, decryptPaths[i]), format);
                    textureImporter.SaveAndReimport();
                }
            }
        }

        public static int MaxSizeTexture(TextureImporter textureImporter, string spritePath)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath);
            int width = texture.width;
            int height = texture.height;

            int maxSize = Mathf.Max(width, height);
            if (maxSize <= 64)
            {
                return 64;
            }
            else if (maxSize <= 128)
            {
                return 128;
            }
            else if (maxSize <= 256)
            {
                return 256;
            }
            else if (maxSize <= 512)
            {
                return 512;
            }
            else if (maxSize <= 1024)
            {
                return 1024;
            }
            else if (maxSize <= 2048)
            {
                return 2048;
            }
            else if (maxSize <= 4096)
            {
                return 4096;
            }
            else if (maxSize <= 8192)
            {
                return 8192;
            }
            else
            {
                return 16384;
            }
        }
    }
}
#endif