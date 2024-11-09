using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Naux.CompressSettingsTool
{
    public class PlatformHandler
    {
        private List<CustomPlatformSettings> lstCustomPlatform;
        public List<CustomPlatformSettings> LstCustomPlatform => lstCustomPlatform;

        public void Initialized()
        {
            lstCustomPlatform = new List<CustomPlatformSettings>()
            {
                new DefaultSettings(),
                new WindowSettings(),
                new AndroidSettings(),
                new IOSSettings(),
                new WebGLSettings()
            };

            for (int i = 0, _count = lstCustomPlatform.Count; i < _count; i++)
            {
                lstCustomPlatform[i].Initialized();
            }
        }

        private CustomPlatformSettings FindPlatform(CustomPlatformType type)
        {
            return lstCustomPlatform.Find(x => x.customPlatformType == type);
        }

        public void DrawCompressSettings(CustomPlatformType type)
        {
            var _platSetting = FindPlatform(type);
            if (_platSetting == null)
            {
                Debug.LogError($"Can't find CustomPlatformType: {type}");
                return;
            }

            _platSetting.CompressSettingsPanel();
        }

        public void HandleMergeSettings(CustomPlatformType type, TextureImporterPlatformSettings textureIPS, int maxSize)
        {
            var _platSetting = FindPlatform(type);
            if (_platSetting == null)
            {
                Debug.LogError($"Can't find CustomPlatformType: {type}");
                return;
            }

            _platSetting.SyncCompressSettings(textureIPS, maxSize);
        }
    }
}