using System.Collections.Generic;
using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public class AndroidSettings : CustomPlatformSettings
    {
        public override void Initialized()
        {
            customPlatformType = CustomPlatformType.Android;
            overridden = false;
            resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            format = TextureImporterFormat.ASTC_4x4;
            compressionQuality = (int)TextureCompressionQuality.Normal;
            allowsAlphaSplitting = false;
            androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;


            messageOverridden = "Override For Android";
            formatDictionary = new Dictionary<TextureImporterFormat, string>()
            {
                { TextureImporterFormat.ASTC_12x12, "RGB(A) Compressed ASTC 12x12 block" },
                { TextureImporterFormat.ASTC_10x10, "RGB(A) Compressed ASTC 10x10 block" },
                { TextureImporterFormat.ASTC_8x8, "RGB(A) Compressed ASTC 8x8 block" },
                { TextureImporterFormat.ASTC_6x6, "RGB(A) Compressed ASTC 6x6 block" },
                { TextureImporterFormat.ASTC_5x5, "RGB(A) Compressed ASTC 5x5 block" },
                { TextureImporterFormat.ASTC_4x4, "RGB(A) Compressed ASTC 4x4 block" },
                { TextureImporterFormat.ETC2_RGBA8, "RGBA Compressed ETC2 8 bits" },
                { TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA, "RGB + 1-bit Alpha Compressed ETC2 4 bits" },
                { TextureImporterFormat.PVRTC_RGBA4, "RGBA Compressed PVRTC 4 bits" },
                { TextureImporterFormat.PVRTC_RGBA2, "RGBA Compressed PVRTC 2 bits" },
                { TextureImporterFormat.DXT5, "RGBA Compressed DXT5|BC3" },
                { TextureImporterFormat.ETC2_RGBA8Crunched, "RGBA Crunched ETC2" },
                { TextureImporterFormat.DXT5Crunched, "RGBA Crunched DXT5|BC3" },
                { TextureImporterFormat.RGBA64, "RGBA 64 bit" },
                { TextureImporterFormat.RGBA16, "RGBA 16 bit" },
                { TextureImporterFormat.RGBA32, "RGBA 32 bit" },
                { TextureImporterFormat.ETC2_RGB4, "RGB Compressed ETC2 4 bits" },
                { TextureImporterFormat.ETC_RGB4, "RGB Compressed ETC 4 bits" },
                { TextureImporterFormat.PVRTC_RGB4, "RGB Compressed PVRTC 4 bits" },
                { TextureImporterFormat.PVRTC_RGB2, "RGB Compressed PVRTC 2 bits" },
                { TextureImporterFormat.DXT1, "RGB Compressed DXT1|BC1" },
                { TextureImporterFormat.ETC_RGB4Crunched, "RGB Crunched ETC" },
                { TextureImporterFormat.DXT1Crunched, "RGB Crunched DXT1|BC1" },
                { TextureImporterFormat.RGB48, "RGB 48 bit" },
                { TextureImporterFormat.RGB16, "RGB 16 bit" },
                { TextureImporterFormat.RGB24, "RGB 24 bit" },
                { TextureImporterFormat.EAC_RG, "RG Compressed EAC 8 bit" },
                { TextureImporterFormat.RG32, "RG 32 bit" },
                { TextureImporterFormat.EAC_R, "R Compressed EAC 4 bit" },
                { TextureImporterFormat.R8, "R 8" },
                { TextureImporterFormat.R16, "R 16 bit" },
                { TextureImporterFormat.Alpha8, "Alpha 8" },
                { TextureImporterFormat.RGBAFloat, "RGBA Float" },
                { TextureImporterFormat.RGBAHalf, "RGBA Half" },
                { TextureImporterFormat.RGFloat, "RG Float" },
                { TextureImporterFormat.RFloat, "R Float" },
                { TextureImporterFormat.ASTC_HDR_12x12, "RGB(A) Compressed ASTC HDR 12x12 block" },
                { TextureImporterFormat.ASTC_HDR_10x10, "RGB(A) Compressed ASTC HDR 10x10 block" },
                { TextureImporterFormat.ASTC_HDR_8x8, "RGB(A) Compressed ASTC HDR 8x8 block" },
                { TextureImporterFormat.ASTC_HDR_6x6, "RGB(A) Compressed ASTC HDR 6x6 block" },
                { TextureImporterFormat.ASTC_HDR_5x5, "RGB(A) Compressed ASTC HDR 5x5 block" },
                { TextureImporterFormat.ASTC_HDR_4x4, "RGB(A) Compressed ASTC HDR 4x4 block" },
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
                textIPSettings.compressionQuality = (int)compressionQuality;

            if (ConditionValidOnSplitAlphaChannel())
                textIPSettings.allowsAlphaSplitting = allowsAlphaSplitting;

            textIPSettings.androidETC2FallbackOverride = androidETC2FallbackOverride;

        }

        protected override void AndroidETC2Fallback()
        {
            androidETC2FallbackOverride = (AndroidETC2FallbackOverride)EditorGUILayout.EnumPopup("Override ETC2 fallback", androidETC2FallbackOverride);
        }

        protected override void CompressorQuality()
        {
            if (!ConditionValidOnCompressor()) return;
            compressionQuality = (int)(TextureCompressionQuality)EditorGUILayout.EnumPopup("Compressor Quality", (TextureCompressionQuality)compressionQuality);

            if (!ConditionValidOnSplitAlphaChannel()) return;
            allowsAlphaSplitting = EditorGUILayout.Toggle("Split Alpha Channel", allowsAlphaSplitting);
        }

        protected override bool ConditionValidOnCompressor()
        {
            return format == TextureImporterFormat.ASTC_12x12
                || format == TextureImporterFormat.ASTC_10x10
                || format == TextureImporterFormat.ASTC_8x8
                || format == TextureImporterFormat.ASTC_6x6
                || format == TextureImporterFormat.ASTC_5x5
                || format == TextureImporterFormat.ASTC_4x4
                || format == TextureImporterFormat.ETC2_RGBA8
                || format == TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA
                || format == TextureImporterFormat.PVRTC_RGBA4
                || format == TextureImporterFormat.PVRTC_RGBA2
                || format == TextureImporterFormat.ETC2_RGBA8Crunched
                || format == TextureImporterFormat.DXT5Crunched
                || format == TextureImporterFormat.ETC2_RGB4
                || format == TextureImporterFormat.ETC_RGB4
                || format == TextureImporterFormat.PVRTC_RGB4
                || format == TextureImporterFormat.PVRTC_RGB2
                || format == TextureImporterFormat.ETC_RGB4Crunched
                || format == TextureImporterFormat.DXT1Crunched
                || format == TextureImporterFormat.ASTC_HDR_12x12
                || format == TextureImporterFormat.ASTC_HDR_10x10
                || format == TextureImporterFormat.ASTC_HDR_8x8
                || format == TextureImporterFormat.ASTC_HDR_6x6
                || format == TextureImporterFormat.ASTC_HDR_5x5
                || format == TextureImporterFormat.ASTC_HDR_4x4;
        }

        protected override bool ConditionValidOnSplitAlphaChannel()
        {
            return format == TextureImporterFormat.ETC_RGB4
                || format == TextureImporterFormat.ETC_RGB4Crunched;
        }
    }
}