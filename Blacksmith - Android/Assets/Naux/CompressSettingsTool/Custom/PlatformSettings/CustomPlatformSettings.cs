using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public abstract class CustomPlatformSettings
    {
        public CustomPlatformType customPlatformType;
        //public int maxTextureSize;
        public TextureResizeAlgorithm resizeAlgorithm;
        public TextureImporterFormat format;

        //public TextureImporterCompression textureCompression = TextureImporterCompression.Compressed;
        public bool crunchedCompression;
        public bool overridden;
        public TextureCompressionQuality compressionQuality;
        public AndroidETC2FallbackOverride androidETC2FallbackOverride;

        public abstract void Initialized();
        public abstract void CompressSettingsPanel();
        public abstract void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize);
    }

    public enum CustomPlatformType
    {
        Default = 0,
        Standalone = 1,
        Android = 2,
        iOS = 3,
        WebGL = 4,
    }
}

