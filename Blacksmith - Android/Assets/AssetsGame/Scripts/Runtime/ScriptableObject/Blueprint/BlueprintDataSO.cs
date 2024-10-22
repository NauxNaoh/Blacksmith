using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = nameof(BlueprintDataSO), menuName = "SO/" + nameof(BlueprintDataSO))]
    public partial class BlueprintDataSO : ScriptableObject
    {
        public List<BlueprintData> lstBlueprintSO;
    }

    [Serializable]
    public class BlueprintData
    {
        public int id;
        public string blueprintName;
        public int sellingPrice;
        public int learnCost;
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