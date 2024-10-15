using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class WebGLSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.WebGL;
            //maxTextureSize = 2048;

            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.DXT5;

            overridden = false;
        }

        public override void CompressSettingsPanel()
        {
            overridden = EditorGUILayout.ToggleLeft("Override For Windows, Mac, Linux", overridden);
            EditorGUILayout.Toggle("Max Size", true);
            resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", resizeAlgorithm);
            format = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", format);
        }

        public override void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize)
        {
            textIPSettings.maxTextureSize = maxSize;
            textIPSettings.resizeAlgorithm = resizeAlgorithm;
            textIPSettings.format = format;

            textIPSettings.overridden = overridden;
        }
    }
}