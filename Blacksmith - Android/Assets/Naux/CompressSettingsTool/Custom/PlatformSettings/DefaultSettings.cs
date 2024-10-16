using System.Collections.Generic;
using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class DefaultSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.Default;
            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.Automatic;
            //missing 1 compression
            crunchedCompression = false;
            compressionQuality = (int)TextureCompressionQuality.Normal;


            formatDictionary = new Dictionary<TextureImporterFormat, string>()
            {
                { TextureImporterFormat.Automatic , "Automatic"},
                { TextureImporterFormat.RGBA32, "RGBA 32 bit" },
                { TextureImporterFormat.RGB16, "RGB 16 bit" },
                { TextureImporterFormat.RGB24, "RGB 24 bit" },
                { TextureImporterFormat.R8, "R 8" },
                { TextureImporterFormat.R16, "R 16 bit" },
                { TextureImporterFormat.Alpha8, "Alpha 8" },
                { TextureImporterFormat.RGBAFloat, "RGBA Float" },
                { TextureImporterFormat.RGBAHalf, "RGBA Half" },
                { TextureImporterFormat.RGFloat, "RG Float" },
                { TextureImporterFormat.RFloat, "R Float" },
                { TextureImporterFormat.RGB9E5, "RGB9e5 32 bit Shared Exponent Float" },
            };
        }

        public override void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize)
        {
            textIPSettings.maxTextureSize = maxSize;
            textIPSettings.resizeAlgorithm = resizeAlgorithm;
            textIPSettings.format = format;

            if (ConditionValidOnCompressor())
            {
                textIPSettings.crunchedCompression = crunchedCompression;
                textIPSettings.compressionQuality = compressionQuality;
            }
        }

        protected override void AndroidETC2Fallback() { }

        protected override void CompressorQuality()
        {
            if (!ConditionValidOnCompressor()) return;
            crunchedCompression = EditorGUILayout.Toggle("Use Crunch Compression", crunchedCompression);

            if (!crunchedCompression) return;
            compressionQuality = (int)(TextureCompressionQuality)EditorGUILayout.EnumPopup("Compressor Quality", (TextureCompressionQuality)compressionQuality);
        }

        protected override bool ConditionValidOnCompressor()
        {
            return format == TextureImporterFormat.Automatic;
        }

        protected override bool ConditionValidOnSplitAlphaChannel()
        {
            return false;
        }
    }
}