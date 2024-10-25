#if UNITY_EDITOR
using Naux.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    public partial class BlueprintDataSO
    {
        [Space, Space, Header("Show in Editor")]
        public string fileCSVName = "BlueprintDataCSV";
        public string pathFromResources = "";

        [ContextMenu(nameof(ReadDataFromCSV))]
        public void ReadDataFromCSV()
        {
            if (string.IsNullOrWhiteSpace(fileCSVName))
            {
                Debug.LogError("File name is NULL");
                return;
            }

            var _content = ImportExportCSV.ReadFromCSV(fileCSVName);
            lstBlueprintSO.Clear();
            lstBlueprintSO = new List<BlueprintSO>();
            for (int i = 1, _count = _content.Length; i < _count; i++)
            {
                var row = _content[i];
                var columns = row.Split(',');
                var _id = 0;

                var _newBlueprint = new BlueprintSO();
                _newBlueprint.id = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.name = Convert.ToString(columns[_id]); _id++;
                _newBlueprint.sellingPrice = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.learnCost = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.idMaterial = Convert.ToInt32(columns[_id]); _id++;

                var _nameSpr = Convert.ToString(columns[_id]);
                var _sprite = GetSpriteFromResources(_nameSpr);
                _newBlueprint.sprite = _sprite;

                lstBlueprintSO.Add(_newBlueprint);
            }

            AssetDatabase.Refresh();
            Debug.Log($"Completed read from CSV");
        }

        public Sprite GetSpriteFromResources(string sprName)
        {
            Sprite _sprite = null;
            if (!string.IsNullOrWhiteSpace(sprName))
            {
                var _path = Path.Combine(pathFromResources, sprName);
                _sprite = Resources.Load<Sprite>(_path);

                if (_sprite == null)
                    Debug.LogError($"Can't find sprName '{sprName}' in folder {pathFromResources}");
                return _sprite;
            }

            Debug.LogError($"Can't find sprName '{sprName}' in folder {pathFromResources}");
            return _sprite;
        }
    }
}
#endif