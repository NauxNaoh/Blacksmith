using UnityEditor;
using UnityEngine;

namespace Naux.CompressSettingsTool
{
    public class CustomTextureImporter
    {
        public TextureImporterType textureType = TextureImporterType.Sprite;
        public bool mipmapEnabled;
        public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
        public FilterMode filterMode = FilterMode.Bilinear;
    }
}