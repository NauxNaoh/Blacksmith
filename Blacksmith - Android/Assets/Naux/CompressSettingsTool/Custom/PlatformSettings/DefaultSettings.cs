using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class DefaultSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.Default;
            //maxTextureSize = 2048;

            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.Automatic;

            //textureCompression = TextureImporterCompression.Compressed;
            crunchedCompression = false;
        }

        public override void CompressSettingsPanel()
        {
            EditorGUILayout.Toggle("Max Size", true);
            resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", resizeAlgorithm);
            format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", format);

            crunchedCompression = EditorGUILayout.Toggle("Use Crunch Compression", crunchedCompression);
        }

        public override void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize)
        {
            textIPSettings.maxTextureSize = maxSize;
            textIPSettings.resizeAlgorithm = resizeAlgorithm;
            textIPSettings.format = format;

            //textIPSettings.textureCompression = textureCompression;
            textIPSettings.crunchedCompression = crunchedCompression;
        }

       
    }
}