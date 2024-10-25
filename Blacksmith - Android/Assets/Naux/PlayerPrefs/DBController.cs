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
        [SerializeField] private InjectionGlobalHelper globalHelper;

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


        #endregion

        void Initializing()
        {
            CheckDependency(DBKey.COIN, key => COIN = 0);
            CheckDependency(DBKey.BLUEPRINT_DB, key =>
            {
                var _temp = new BlueprintDB();
                _temp.lstBlueprintInfo = new List<BlueprintModel>();

                var _lstDataSO = globalHelper.BlueprintDataSO.lstBlueprintSO;

                BlueprintModel _blueprintInfo;
                for (int i = 0, _count = _lstDataSO.Count; i < _count; i++)
                {
                    _blueprintInfo = new BlueprintModel
                    {
                        id = _lstDataSO[i].id,
                        isLock = _lstDataSO[i].learnCost > 0
                    };


                    _temp.lstBlueprintInfo.Add(_blueprintInfo);
                }

                BLUEPRINT_DB = _temp;
            });

            Load();
        }

        void Load()
        {
            coin = LoadDataByKey<int>(DBKey.COIN);
            blueprintDB = LoadDataByKey<BlueprintDB>(DBKey.BLUEPRINT_DB);
        }
    }

    internal class DBKey
    {
        public static readonly string COIN = "COIN";
        public static readonly string BLUEPRINT_DB = "BLUEPRINT_DB";
    }
}