using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class IOSSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.iOS;
            //axTextureSize = 2048;

            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.ASTC_6x6;

            overridden = false;
            compressionQuality = TextureCompressionQuality.Normal;
        }

        public override void CompressSettingsPanel()
        {
            overridden = EditorGUILayout.ToggleLeft("Override For iOS", overridden);
            EditorGUILayout.Toggle("Max Size", true);
            resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", resizeAlgorithm);
            format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", format);
            compressionQuality = (TextureCompressionQuality)EditorGUILayout.EnumPopup("Compressor Quality", compressionQuality);
        }

        public override void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize)
        {
            textIPSettings.maxTextureSize = maxSize;
            textIPSettings.resizeAlgorithm = resizeAlgorithm;
            textIPSettings.format = format;

            textIPSettings.overridden = overridden;
            textIPSettings.compressionQuality = (int)compressionQuality;
        }
    }
}