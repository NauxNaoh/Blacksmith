using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class AndroidSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.Android;
            //maxTextureSize = 2048;

            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.ASTC_6x6;

            overridden = false;
            compressionQuality = TextureCompressionQuality.Normal;
            androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
        }

        public override void CompressSettingsPanel()
        {
            overridden = EditorGUILayout.ToggleLeft("Override For Android", overridden);
            EditorGUILayout.Toggle("Max Size", true);
            resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", resizeAlgorithm);
            format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", format);
            compressionQuality = (TextureCompressionQuality)EditorGUILayout.EnumPopup("Compressor Quality", compressionQuality);
            androidETC2FallbackOverride = (AndroidETC2FallbackOverride)EditorGUILayout.EnumPopup("Override ETC2 fallback", androidETC2FallbackOverride);
        }

        public override void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize)
        {
            textIPSettings.maxTextureSize = maxSize;
            textIPSettings.resizeAlgorithm = resizeAlgorithm;
            textIPSettings.format = format;

            textIPSettings.overridden = overridden;
            textIPSettings.compressionQuality = (int)compressionQuality;
            textIPSettings.androidETC2FallbackOverride = androidETC2FallbackOverride;
        }
    }
}