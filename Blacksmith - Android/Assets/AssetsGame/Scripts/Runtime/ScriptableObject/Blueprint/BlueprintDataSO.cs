using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = nameof(BlueprintDataSO), menuName = "SO/" + nameof(BlueprintDataSO))]
    public partial class BlueprintDataSO : ScriptableObject
    {
        public List<BlueprintSO> lstBlueprintSO;

        public BlueprintSO FindBlueprintWithID(int id)
        {
           return lstBlueprintSO.Find(x => x.id == id);
        }
    }

    [Serializable]
    public class BlueprintSO
    {
        public int id;
        public string name;
        public int sellingPrice;
        public int learnCost;
        public int idMaterial;
        public Sprite sprite;
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