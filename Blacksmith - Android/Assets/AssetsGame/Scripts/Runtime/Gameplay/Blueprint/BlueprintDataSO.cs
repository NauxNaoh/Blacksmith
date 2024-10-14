using Naux.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = nameof(BlueprintDataSO), menuName = "SO/" + nameof(BlueprintDataSO))]
    public class BlueprintDataSO : ScriptableObject
    {
        public string fileNameCSV = "export";
        public List<BlueprintData> blueprintsDataSO;
        public List<SpriteMaterials> SpriteMaterialsSO;


#if UNITY_EDITOR
        [ContextMenu(nameof(ReadFromCSV))]
        public void ReadFromCSV()
        {
            if (string.IsNullOrWhiteSpace(fileNameCSV))
            {
                Debug.LogError("File name is NULL");
                return;
            }
            var _content = ImportExportCSV.ReadFromCSV(fileNameCSV);

            blueprintsDataSO = new List<BlueprintData>();
            BlueprintData _newBlueprint = null;
            for (int i = 1, _count = _content.Length; i < _count; i++)
            {
                var row = _content[i];
                var columns = row.Split(',');


                if (!string.IsNullOrWhiteSpace(columns[0]))
                {
                    var _id = 0;
                    _newBlueprint = new BlueprintData();
                    _newBlueprint.id = Convert.ToInt32(columns[_id]); _id++;
                    _newBlueprint.blueprintType = (BlueprintType)Enum.Parse(typeof(BlueprintType), columns[_id]); _id++;
                    _newBlueprint.nameBlueprint = Convert.ToString(columns[_id]); _id++;
                    _newBlueprint.price = Convert.ToInt32(columns[_id]); _id++;
                    _newBlueprint.nameSprite = Convert.ToString(columns[_id]); _id++;
                    _newBlueprint.materials = new List<MaterialInfo>();

                    if (!string.IsNullOrWhiteSpace(columns[_id]))
                    {
                        var _newMaterial = new MaterialInfo();
                        _newMaterial.materialType = (MaterialType)Enum.Parse(typeof(MaterialType), columns[_id]); _id++;
                        _newMaterial.amount = Convert.ToInt32(columns[_id]);

                        _newBlueprint.materials.Add(_newMaterial);
                    }

                    blueprintsDataSO.Add(_newBlueprint);
                }
                else if (_newBlueprint != null)
                {
                    var _id = 5;
                    if (!string.IsNullOrWhiteSpace(columns[_id]))
                    {
                        var _newMaterial = new MaterialInfo();
                        _newMaterial.materialType = (MaterialType)Enum.Parse(typeof(MaterialType), columns[_id]); _id++;
                        _newMaterial.amount = Convert.ToInt32(columns[_id]);

                        _newBlueprint.materials.Add(_newMaterial);
                    }
                }
            }

            AssetDatabase.Refresh();
            Debug.Log($"Completed read from CSV");
        }

        [ContextMenu(nameof(WriteToCSV))]
        public void WriteToCSV()
        {
            if (string.IsNullOrWhiteSpace(fileNameCSV))
            {
                Debug.LogError("File name is NULL");
                return;
            }

            var _content = WriteToStringBuilder();
            ImportExportCSV.WriteToCSV(fileNameCSV, _content);
            Debug.Log($"Completed write to CSV");
        }
        string WriteToStringBuilder()
        {
            var _stringBuilder = new StringBuilder(
                $"{nameof(BlueprintData.id)}," +
                $"{nameof(BlueprintData.blueprintType)}," +
                $"{nameof(BlueprintData.nameBlueprint)}," +
                $"{nameof(BlueprintData.price)}," +
                $"{nameof(BlueprintData.nameSprite)}," +
                $"{nameof(MaterialInfo.materialType)}," +
                $"{nameof(MaterialInfo.amount)}");

            var _dataSO = blueprintsDataSO;
            for (int i = 0, _countBlueprint = _dataSO.Count; i < _countBlueprint; i++)
            {
                var _countMaterials = _dataSO[i].materials.Count;
                if (_countMaterials > 0)
                {
                    var _firstMaterial = true;
                    for (int j = 0; j < _countMaterials; j++)
                    {
                        if (_firstMaterial)
                        {
                            _stringBuilder.AppendLine()
                            .Append($"{_dataSO[i].id}").Append(',')
                            .Append($"{_dataSO[i].blueprintType}").Append(',')
                            .Append($"{_dataSO[i].nameBlueprint}").Append(',')
                            .Append($"{_dataSO[i].price}").Append(',')
                            .Append($"{_dataSO[i].nameSprite}").Append(',')
                            .Append($"{_dataSO[i].materials[j].materialType}").Append(',')
                            .Append($"{_dataSO[i].materials[j].amount}").Append(',');

                            _firstMaterial = false;
                        }
                        else
                        {
                            _stringBuilder.AppendLine()
                            .Append(',', 5)
                            .Append($"{_dataSO[i].materials[j].materialType}").Append(',')
                            .Append($"{_dataSO[i].materials[j].amount}").Append(',');
                        }

                    }
                }
                else
                {
                    _stringBuilder.AppendLine()
                    .Append($"{_dataSO[i].id}").Append(',')
                    .Append($"{_dataSO[i].blueprintType}").Append(',')
                    .Append($"{_dataSO[i].nameBlueprint}").Append(',')
                    .Append($"{_dataSO[i].price}").Append(',')
                    .Append($"{_dataSO[i].nameSprite}").Append(',');
                }

            }

            return _stringBuilder.ToString();
        }
#endif
    }

    [Serializable]
    public class BlueprintData
    {
        public int id;
        public BlueprintType blueprintType;
        public string nameBlueprint;
        public int price;
        public string nameSprite;
        public List<MaterialInfo> materials;
    }

    [Serializable]
    public class MaterialInfo
    {
        public MaterialType materialType;
        public int amount;
    }

    [Serializable]
    public class SpriteMaterials
    {
        public MaterialType materialType;
        public Sprite sprite;
    }

    public enum BlueprintType
    {
        None = 0,
        CaiBua = 1,
        CaiThia = 2,
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

