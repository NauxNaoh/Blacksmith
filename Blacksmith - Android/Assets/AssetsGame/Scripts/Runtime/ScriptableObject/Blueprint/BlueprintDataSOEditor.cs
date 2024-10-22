#if UNITY_EDITOR
using Naux.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    public partial class BlueprintDataSO
    {
        [Space,Space, Header("Show in Editor")]
        public string fileCSVName = "export";
        public string pathFromResources = "";

        [ContextMenu(nameof(ReadFromCSV))]
        public void ReadFromCSV()
        {
            if (string.IsNullOrWhiteSpace(fileCSVName))
            {
                Debug.LogError("File name is NULL");
                return;
            }
            var _content = ImportExportCSV.ReadFromCSV(fileCSVName);
            lstBlueprintSO.Clear();
            lstBlueprintSO = new List<BlueprintData>();
            for (int i = 1, _count = _content.Length; i < _count; i++)
            {
                var row = _content[i];
                var columns = row.Split(',');
                var _id = 0;

                var _newBlueprint = new BlueprintData();
                _newBlueprint.id = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.blueprintName = Convert.ToString(columns[_id]); _id++;
                _newBlueprint.sellingPrice = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.learnCost = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.materialType = (MaterialType)Enum.Parse(typeof(MaterialType), columns[_id]); _id++;
                _newBlueprint.nameSprite = Convert.ToString(columns[_id]);
                lstBlueprintSO.Add(_newBlueprint);
            }

            AssetDatabase.Refresh();
            Debug.Log($"Completed read from CSV");
        }


        [ContextMenu(nameof(WriteToCSV))]
        public void WriteToCSV()
        {
            if (string.IsNullOrWhiteSpace(fileCSVName))
            {
                Debug.LogError("File name is NULL");
                return;
            }

            var _content = WriteToStringBuilder();
            ImportExportCSV.WriteToCSV(fileCSVName, _content);
            Debug.Log($"Completed write to CSV");
        }
        string WriteToStringBuilder()
        {
            var _stringBuilder = new StringBuilder(
                $"{nameof(BlueprintData.id)}," +
                $"{nameof(BlueprintData.blueprintName)}," +
                $"{nameof(BlueprintData.sellingPrice)}," +
                $"{nameof(BlueprintData.learnCost)}," +
            $"{nameof(BlueprintData.materialType)}," +
                $"{nameof(BlueprintData.nameSprite)}");
            for (int i = 0; i < lstBlueprintSO.Count; i++)
            {

                _stringBuilder.AppendLine()
                .Append($"{lstBlueprintSO[i].id}").Append(',')
                .Append($"{lstBlueprintSO[i].blueprintName}").Append(',')
                .Append($"{lstBlueprintSO[i].sellingPrice}").Append(',')
                .Append($"{lstBlueprintSO[i].learnCost}").Append(',')
                .Append($"{lstBlueprintSO[i].materialType}").Append(',')
                .Append($"{lstBlueprintSO[i].nameSprite}").Append(',');
            }

            return _stringBuilder.ToString();
        }

        [ContextMenu(nameof(AutoGetSprite))]
        public void AutoGetSprite()
        {
            for (int i = 0, _count = lstBlueprintSO.Count; i < _count; i++)
            {
                var _nameSpr = lstBlueprintSO[i].nameSprite;
                if (string.IsNullOrWhiteSpace(_nameSpr))
                {
                    Debug.LogError($"No data or wrong nameSprite at blueprint {lstBlueprintSO[i].id}");
                    continue;
                }

                var _path = Path.Combine(pathFromResources, _nameSpr);
                var _sprite = Resources.Load<Sprite>(_path);
                if (_sprite != null)
                    lstBlueprintSO[i].blueprintSprite = _sprite;
                else
                    Debug.LogError($"Sprite name: {_nameSpr} not found " +
                        $"in {pathFromResources} " +
                        $"for blueprint {lstBlueprintSO[i].id}");
            }

            AssetDatabase.Refresh();
        }
    }
}
#endif