using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TST
{
    public interface ILoader<Key, Value>
    {
        public Dictionary<Key,Value> MakeDict();
    }

    [System.Serializable]
    public class SaveLoadDataWrapper<T> : ILoader<int, T> where T : RootDataDTO
    {
        public List<T> Values;

        public Dictionary<int, T> MakeDict()
        {
            Dictionary<int, T> dict = new Dictionary<int, T>();
            foreach(T value in Values)
                dict.Add(value.ID, value);
            return dict;
        }
    }

    public class UserDataModel : SingletonBase<UserDataModel>
    {
        [field: SerializeField] public Dictionary<int, IngamePlayerDataDTO> IngamePlayerData { get; private set; } = new Dictionary<int, IngamePlayerDataDTO> ();

        public void Initialize()
        {
            // 즨짜아아으아아아 머리아파 ㅠㅠㅋㅋ 
            IngamePlayerData = LoadData<IngamePlayerDataDTO>().MakeDict();
        }

        public void SaveIngamePlayerData(Vector3 position, Quaternion rotation)
        {
            //IngamePlayerData["Tory"].Position = position;

            //SaveData(IngamePlayerData);
        }

        #region SAVE / LOAD Core Method

        public SaveLoadDataWrapper<T> LoadData<T>() where T : RootDataDTO
        {
#if UNITY_EDITOR
            string path = $"Assets/PROJECT TST/Anothers/Editor Saved Data/Json/{typeof(T).Name}.json";
#else
            string path = $"{Application.persistentDataPath}/{innerType.Name}.json";
#endif
            if (FileManager.ReadFileData(path, out string loadedEditorData))
            {
                // JSON 역직렬화
                var wrapper = JsonConvert.DeserializeObject<SaveLoadDataWrapper<T>>(loadedEditorData);

                return wrapper;
            }
            
            Debug.Log($"Failed to Load Data {typeof(T).Name}");
            return null;
        }

        public void SaveData<T>(Dictionary<string, T> newData) where T : RootDataDTO
        {
#if UNITY_EDITOR
            string jsonPath = $"Assets/PROJECT TST/Anothers/Editor Saved Data/Json/{typeof(T).Name}.json";
            string csvPath = $"Assets/PROJECT TST/Anothers/Editor Saved Data/Csv/{typeof(T).Name}.csv";
#else
            string jsonPath = $"{Application.persistentDataPath}/{typeof(T).Name}.json";
            string csvPath = $"{Application.persistentDataPath}/{typeof(T).Name}.csv";
#endif

            // JSON 저장
            string jsonData = "";
            foreach (var dic in newData)
            {
                string key = dic.Key;
                jsonData = JsonUtility.ToJson(newData[key], true);
                FileManager.WriteFileFromString(jsonPath, jsonData);
            }
            Debug.Log($"Save Data to JSON Success: {jsonData}");

            // CSV 저장
            SaveToCsv(newData, csvPath);
            Debug.Log($"Save Data to CSV Success: {csvPath}");
        }

        public static void SaveToCsv<T>(T data, string filePath)
        {
            var properties = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var csvBuilder = new StringBuilder();

            // 헤더 생성
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // 데이터 추가
            var values = properties.Select(p =>
            {
                var value = p.GetValue(data);

                if (value is IEnumerable enumerable && !(value is string))
                {
                    return $"\"{string.Join("&", enumerable.Cast<object>())}\""; // 리스트는 '&'로 구분, 큰따옴표로 감싸기
                }
                // Vector3 처리
                else if (value is Vector3 vector)
                {
                    return $"\"({vector.x},{vector.y},{vector.z})\""; // x, y, z 형식을 큰따옴표로 감싸기
                }
                // Quaternion 처리
                else if (value is Quaternion quaternion)
                {
                    return $"\"({quaternion.x},{quaternion.y},{quaternion.z},{quaternion.w})\""; // x, y, z, w 형식
                }
                // 일반 데이터
                else
                {
                    return value?.ToString()?.Replace(",", " ").Replace("\"", "\"\""); // 쉼표 제거 및 큰따옴표 이스케이프
                }
            });

            csvBuilder.AppendLine(string.Join(",", values));

            // 파일 저장
            File.WriteAllText(filePath, csvBuilder.ToString());
        }
        #endregion


    }
}
