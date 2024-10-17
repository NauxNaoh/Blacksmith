using Naux.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Runtime
{
    [CreateAssetMenu(fileName = nameof(BlueprintDataSO), menuName = "SO/" + nameof(BlueprintDataSO))]
    public class BlueprintDataSO : ScriptableObject
    {
        public List<BlueprintData> blueprintsDataSO;



#if UNITY_EDITOR
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
            blueprintsDataSO.Clear();
            blueprintsDataSO = new List<BlueprintData>();
            BlueprintData _newBlueprint;
            for (int i = 1, _count = _content.Length; i < _count; i++)
            {
                var row = _content[i];
                var columns = row.Split(',');

                var _id = 0;
                _newBlueprint = new BlueprintData();
                _newBlueprint.id = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.blueprintName = Convert.ToString(columns[_id]); _id++;
                _newBlueprint.price = Convert.ToInt32(columns[_id]); _id++;
                _newBlueprint.materialType = (MaterialType)Enum.Parse(typeof(MaterialType), columns[_id]); _id++;
                _newBlueprint.nameSprite = Convert.ToString(columns[_id]); ;
                blueprintsDataSO.Add(_newBlueprint);
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
                $"{nameof(BlueprintData.price)}," +
                $"{nameof(BlueprintData.materialType)}," +
                $"{nameof(BlueprintData.nameSprite)}");
            for (int i = 0; i < blueprintsDataSO.Count; i++)
            {

                _stringBuilder.AppendLine()
                .Append($"{blueprintsDataSO[i].id}").Append(',')
                .Append($"{blueprintsDataSO[i].blueprintName}").Append(',')
                .Append($"{blueprintsDataSO[i].price}").Append(',')
                .Append($"{blueprintsDataSO[i].materialType}").Append(',')
                .Append($"{blueprintsDataSO[i].nameSprite}").Append(',');
            }

            return _stringBuilder.ToString();
        }

        [ContextMenu(nameof(AutoGetSprite))]
        public void AutoGetSprite()
        {
            for (int i = 0, _count = blueprintsDataSO.Count; i < _count; i++)
            {
                var _nameSpr = blueprintsDataSO[i].nameSprite;
                if (string.IsNullOrWhiteSpace(_nameSpr))
                {
                    Debug.LogError($"No data or wrong nameSprite at blueprint {blueprintsDataSO[i].id}");
                    continue;
                }

                var _path = Path.Combine(pathFromResources, _nameSpr);
                var _sprite = Resources.Load<Sprite>(_path);
                if (_sprite != null)
                    blueprintsDataSO[i].blueprintSprite = _sprite;
                else
                    Debug.LogError($"Sprite name: {_nameSpr} not found " +
                        $"in {pathFromResources} " +
                        $"for blueprint {blueprintsDataSO[i].id}");
            }

            AssetDatabase.Refresh();
        }
#endif
    }

    [Serializable]
    public class BlueprintData
    {
        public int id;
        public string blueprintName;
        public int price;
        public MaterialType materialType;
        public Sprite blueprintSprite;
        public string nameSprite;
    }

    public enum MaterialType
    {
        None = 0,
        IronTier1 = 1,
        IronTier2 = 2,
        WoodTier1 = 3,
        WoodTier2 = 4,
    }
}