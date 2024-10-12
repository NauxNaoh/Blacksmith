using Naux.Patterns;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Naux.DB
{
    public class DBController : Singleton<DBController>
    {
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
                ;
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
        #endregion

        void Initializing()
        {
            CheckDependency(DBKey.COIN, key => coin = 0);

            Load();
        }

        void Load()
        {
            coin = LoadDataByKey<int>(DBKey.COIN);
        }
    }

    internal class DBKey
    {
        public static readonly string COIN = "COIN";
    }
}