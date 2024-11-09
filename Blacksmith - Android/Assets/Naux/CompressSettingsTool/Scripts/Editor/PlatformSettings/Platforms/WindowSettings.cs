using System.Collections.Generic;
using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class WindowSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.Standalone;
            overridden = false;
            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.DXT1;
            compressionQuality = (int)TextureCompressionQuality.Normal;


            messageOverridden = "Override For Windows, Mac, Linux";
            formatDictionary = new Dictionary<TextureImporterFormat, string>()
            {
                { TextureImporterFormat.BC7 , "RGB(A) Compressed BC7"},
                { TextureImporterFormat.DXT5 , "RGBA Compressed DXT5|BC3"},
                { TextureImporterFormat.DXT5Crunched , "RGBA Crunched DXT5|BC3"},
                { TextureImporterFormat.RGBA64 , "RGBA 64 bit"},
                { TextureImporterFormat.RGBA32 , "RGBA 32 bit"},
                { TextureImporterFormat.ARGB16 , "ARGB 16 bit"},
                { TextureImporterFormat.DXT1 , "RGB Compressed DXT1|BC1"},
                { TextureImporterFormat.DXT1Crunched, "RGB Crunched DXT1|BC1"},
                { TextureImporterFormat.RGB48, "RGB 48 bit" },
                { TextureImporterFormat.RGB16, "RGB 16 bit" },
                { TextureImporterFormat.RGB24, "RGB 24 bit" },
                { TextureImporterFormat.BC5, "RG Compressed BC5" },
                { TextureImporterFormat.RG32, "RG 32 bit" },
                { TextureImporterFormat.BC4, "R Compressed BC4" },
                { TextureImporterFormat.R8, "R 8" },
                { TextureImporterFormat.R16, "R 16 bit" },
                { TextureImporterFormat.Alpha8, "Alpha 8" },
                { TextureImporterFormat.RGBAFloat, "RGBA Float" },
                { TextureImporterFormat.RGBAHalf, "RGBA Half" },
                { TextureImporterFormat.RGFloat, "RG Float" },
                { TextureImporterFormat.RFloat, "R Float" },
                { TextureImporterFormat.BC6H, "RGB HDR Compressed BC6H" },
                { TextureImporterFormat.RGB9E5, "RGB9e5 32 bit Shared Exponent Float" },
            };
        }

        public override void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize)
        {
            textIPSettings.overridden = overridden;
            textIPSettings.maxTextureSize = maxSize;
            textIPSettings.resizeAlgorithm = resizeAlgorithm;
            textIPSettings.format = format;

            if (ConditionValidOnCompressor())
            {
                textIPSettings.compressionQuality = compressionQuality;
            }
        }

        protected override void AndroidETC2Fallback() { }

        protected override void CompressorQuality()
        {
            if (!ConditionValidOnCompressor()) return;
            compressionQuality = (int)(TextureCompressionQuality)EditorGUILayout.EnumPopup("Compressor Quality", (TextureCompressionQuality)compressionQuality);
        }

        protected override bool ConditionValidOnCompressor()
        {
            return format == TextureImporterFormat.BC7
                || format == TextureImporterFormat.DXT5Crunched
                || format == TextureImporterFormat.DXT1Crunched
                || format == TextureImporterFormat.BC6H;
        }

        protected override bool ConditionValidOnSplitAlphaChannel()
        {
            return false;
        }
    }
}