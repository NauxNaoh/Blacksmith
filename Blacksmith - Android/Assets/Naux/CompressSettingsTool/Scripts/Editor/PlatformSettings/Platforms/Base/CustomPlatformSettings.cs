using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Naux.CompressSettingsTool
{
    public abstract class CustomPlatformSettings
    {
        public CustomPlatformType customPlatformType;
        public bool overridden;
        public TextureResizeAlgorithm resizeAlgorithm;
        public TextureImporterFormat format;
        public bool crunchedCompression;
        public int compressionQuality;
        public bool allowsAlphaSplitting;
        public AndroidETC2FallbackOverride androidETC2FallbackOverride;


        public string messageOverridden;
        public Dictionary<TextureImporterFormat, string> formatDictionary;


        public abstract void Initialized();
        public abstract void SyncCompressSettings(TextureImporterPlatformSettings textIPSettings, int maxSize);
        public void CompressSettingsPanel()
        {
            OverridePlatform();
            MaxSize();
            ResizeAlgorithm();
            Format();
            CompressorQuality();
            AndroidETC2Fallback();
        }
        void OverridePlatform()
        {
            if (customPlatformType == CustomPlatformType.Default) return;
            overridden = EditorGUILayout.ToggleLeft(messageOverridden, overridden);
        }

        void MaxSize()
        {
            EditorGUILayout.Toggle("Max Size", true);
        }

        void ResizeAlgorithm()
        {
            resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup("Resize Algorithm", resizeAlgorithm);
        }

        void Format()
        {
            TextureImporterFormat[] _optionsArray = formatDictionary.Keys.ToArray();
            string[] _displayNames = formatDictionary.Values.ToArray();
            var _currentIndex = Array.IndexOf(_optionsArray, format);
            _currentIndex = EditorGUILayout.Popup("Format", _currentIndex, _displayNames);
            format = _optionsArray[_currentIndex];
        }

        protected abstract void AndroidETC2Fallback();
        protected abstract void CompressorQuality();
        protected abstract bool ConditionValidOnCompressor();
        protected abstract bool ConditionValidOnSplitAlphaChannel();
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

