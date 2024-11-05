using Naux.Patterns;
using Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Naux.DB
{
    public class DBController : Singleton<DBController>
    {
        [SerializeField] private GlobalInjectionHelper globalHelper;

        #region Default
        protected override void Awake()
        {
            base.Awake();
            Initializing();
        }

        void CheckDependency(string key, UnityAction<string> onComplete)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                onComplete?.Invoke(key);
            }
        }

        void Save<T>(string key, T values)
        {
            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(bool) ||
                typeof(T) == typeof(string) ||
                typeof(T) == typeof(float) ||
                typeof(T) == typeof(long) ||
                typeof(T) == typeof(Quaternion) ||
                typeof(T) == typeof(Vector2) ||
                typeof(T) == typeof(Vector3) ||
                typeof(T) == typeof(Vector2Int) ||
                typeof(T) == typeof(Vector3Int))
            {
                PlayerPrefs.SetString(key, $"{values}");
            }
            else
            {
                try
                {
                    string json = JsonUtility.ToJson(values);
                    PlayerPrefs.SetString(key, json);
                }
                catch (UnityException e)
                {
                    throw new UnityException(e.Message);
                }
            }
        }

        T LoadDataByKey<T>(string key)
        {
            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(bool) ||
                typeof(T) == typeof(string) ||
                typeof(T) == typeof(float) ||
                typeof(T) == typeof(long) ||
                typeof(T) == typeof(Quaternion) ||
                typeof(T) == typeof(Vector2) ||
                typeof(T) == typeof(Vector3) ||
                typeof(T) == typeof(Vector2Int) ||
                typeof(T) == typeof(Vector3Int))
            {
                var value = PlayerPrefs.GetString(key);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                var json = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(json);
            }
        }

        #endregion

        #region Declare
        private int coin;
        internal int COIN
        {
            get => coin;
            set
            {
                coin = value;
                Save(DBKey.COIN, value);
            }
        }

        private int totalCoin;
        internal int TOTAL_COIN
        {
            get => totalCoin;
            set
            {
                totalCoin = value;
                Save(DBKey.TOTAL_COIN, value);
            }
        }
        [SerializeField]

        private BlueprintDB blueprintDB;
        internal BlueprintDB BLUEPRINT_DB
        {
            get => blueprintDB;
            set
            {
                blueprintDB = value;
                Save(DBKey.BLUEPRINT_DB, value);
            }
        }
        [SerializeField]
        private MissionDB missionDB;
        internal MissionDB MISSION_DB
        {
            get => missionDB;
            set
            {
                missionDB = value;
                Save(DBKey.MISSION_DB, value);
            }
        }

        #endregion

        void Initializing()
        {
            CheckDependency(DBKey.COIN, key => COIN = 0);
            CheckDependency(DBKey.TOTAL_COIN, key => TOTAL_COIN = 0);
            CheckDependency(DBKey.BLUEPRINT_DB, key =>
            {
                var _temp = new BlueprintDB();
                _temp.lstBlueprint = new List<BlueprintModel>();

                var _lstBlueprintSO = globalHelper.BlueprintDataSO.lstBlueprintSO;

                BlueprintModel _blueprint;
                for (int i = 0, _count = _lstBlueprintSO.Count; i < _count; i++)
                {
                    _blueprint = new BlueprintModel
                    {
                        idBlueprint = _lstBlueprintSO[i].id,
                        isLock = _lstBlueprintSO[i].learnCost > 0
                    };


                    _temp.lstBlueprint.Add(_blueprint);
                }

                BLUEPRINT_DB = _temp;
                //Doesn't Handle Update Version
            });
            CheckDependency(DBKey.MISSION_DB, key =>
            {
                var _temp = new MissionDB();
                _temp.idLastMission = -1;
                _temp.lstMission = new List<MissionModel>();

                var _lstBlueprintSO = globalHelper.BlueprintDataSO.lstBlueprintSO;
                MissionModel _mission;
                for (int i = 0, _count = _lstBlueprintSO.Count; i < _count; i++)
                {
                    if (i >= 2) break;

                    _temp.idLastMission += 1;
                    _mission = new MissionModel
                    {
                        idMission = _temp.idLastMission,
                        idBlueprint = _lstBlueprintSO[i].id,
                        amountRequest = 2
                    };
                    
                    _temp.lstMission.Add(_mission);
                }

                MISSION_DB = _temp;
                //Doesn't Handle Update Version
            });
            Load();
        }

        void Load()
        {
            coin = LoadDataByKey<int>(DBKey.COIN);
            totalCoin = LoadDataByKey<int>(DBKey.TOTAL_COIN);
            blueprintDB = LoadDataByKey<BlueprintDB>(DBKey.BLUEPRINT_DB);
            missionDB = LoadDataByKey<MissionDB>(DBKey.MISSION_DB);
        }
    }

    internal class DBKey
    {
        public static readonly string COIN = "COIN";
        public static readonly string TOTAL_COIN = "TOTAL_COIN";
        public static readonly string BLUEPRINT_DB = "BLUEPRINT_DB";
        public static readonly string MISSION_DB = "MISSION_DB";
    }
}