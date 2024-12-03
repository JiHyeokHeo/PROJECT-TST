using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class UserDataModel : SingletonBase<UserDataModel>
    {
        [field: SerializeField] public IngamePlayerDataDTO IngamePlayerData { get; private set; } = new IngamePlayerDataDTO();

        public void Initialize()
        {
            if (LoadData(out IngamePlayerDataDTO loadPlayerData))
            {
                IngamePlayerData = loadPlayerData;
            }
            else // Editor 에서 저장한 데이터가 없을 때.
            {
                IngamePlayerData = new IngamePlayerDataDTO();
                SaveData(IngamePlayerData);
            }
        }

        //public void SaveIngamePlayerData(Vector3 position, Quaternion rotation)
        //{
        //    IngamePlayerData.PlayerPosition = position;
        //    IngamePlayerData.PlayerRotation = rotation;

        //    SaveData(IngamePlayerData);
        //}

        #region SAVE / LOAD Core Method

        public bool LoadData<T>(out T loadedData) where T : UserDataDTO
        {
#if UNITY_EDITOR            
            string path = $"Assets/PROJECT TST/Anothers/Editor Saved Data/Json/{typeof(T).Name}.json";
#else
            string path = $"{Application.persistentDataPath}/{typeof(T).Name}.json";
#endif
            if (FileManager.ReadFileData(path, out string loadedEditorData))
            {
                loadedData = JsonUtility.FromJson<T>(loadedEditorData);
                return true;
            }

            loadedData = null;
            Debug.Log($"Failed to Load Data {typeof(T).Name}");
            return false;
        }

        public void SaveData<T>(T newData) where T : UserDataDTO
        {
#if UNITY_EDITOR
            string path = $"Assets/PROJECT TST/Anothers/Editor Saved Data/Json/{typeof(T).Name}.json";
#else
            string path = $"{Application.persistentDataPath}/{typeof(T).Name}.json";
#endif
            string jsonData = JsonUtility.ToJson(newData, true);
            FileManager.WriteFileFromString(path, jsonData);

            Debug.Log($"Save Data Success : {jsonData}");
        }

        #endregion
    }
}
