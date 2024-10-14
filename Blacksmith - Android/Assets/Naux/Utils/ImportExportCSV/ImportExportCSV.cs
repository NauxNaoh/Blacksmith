using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Naux.Utils
{
    public static class ImportExportCSV
    {
        public static void WriteToCSV(string fileName, string content)
        {
            var _folder = Application.streamingAssetsPath;
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            var _filePath = Path.Combine(_folder, fileName + ".csv");
            using (var writer = new StreamWriter(_filePath, false))
            {
                writer.Write(content);
            }
            //File.WriteAllText(_filePath, _content);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static string[] ReadFromCSV(string fileName)
        {
            var _folder = Application.streamingAssetsPath;
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            var _filePath = Path.Combine(_folder, fileName + ".csv");
            if (!File.Exists(_filePath))
            {
                Debug.LogError($"File not exists in path: {_filePath}");
                return null;
            }

            var _content = File.ReadAllLines(_filePath);
            return _content;
        }
    }
}
