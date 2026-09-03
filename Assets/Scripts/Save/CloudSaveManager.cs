using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    public class LoadResult<T>
    {
        public bool Success;
        public bool HasData;
        public T Data;
        public string ErrorMessage;
    }

    public static async Task<bool> SaveDataOnCloud<T>(string key, T inData)
    {
        try {
            string dataString = JsonUtility.ToJson(inData);
            var data = new Dictionary<string, object> { { key, dataString } };

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log($"[CloudSave] Save success : {key}");
            return true;
        } catch(Exception e) {
            Debug.LogError($"[CloudSave] Failed to save data : {e}");
            return false;
        }
    }

    public static async Task<LoadResult<GameData>> LoadDataOnCloud(string key)
    {
        var result = new LoadResult<GameData> {
            Success = false,
            HasData = false,
            Data = null,
            ErrorMessage = ""
        };

        try {
            var savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });

            if(!savedData.ContainsKey(key)) {
                Debug.LogWarning($"[CloudSave] Key not found : {key}");
                result.Success = true;
                result.HasData = false;
                return result;
            }

            string dataString = savedData[key];

            if(string.IsNullOrEmpty(dataString)) {
                Debug.LogWarning($"[CloudSave] Data string is null or empty : {key}");
                result.Success = true;
                result.HasData = false;
                return result;
            }

            GameData loadedData = JsonUtility.FromJson<GameData>(dataString);

            if(loadedData == null) {
                Debug.LogError($"[CloudSave] Json parse failed : {key}");
                result.Success = false;
                result.HasData = false;
                result.ErrorMessage = "Json parse failed";
                return result;
            }

            result.Success = true;
            result.HasData = true;
            result.Data = loadedData;

            Debug.Log($"[CloudSave] Load success : {key}");
            return result;
        } catch(Exception e) {
            Debug.LogError($"[CloudSave] Failed to load data : {e}");
            result.Success = false;
            result.HasData = false;
            result.ErrorMessage = e.Message;
            return result;
        }
    }

    public static async Task<bool> DeleteSomeData(string key)
    {
        try {
            await CloudSaveService.Instance.Data.ForceDeleteAsync(key);
            Debug.Log($"[CloudSave] Delete success : {key}");
            return true;
        } catch(Exception e) {
            Debug.LogError($"[CloudSave] Failed to delete data : {e}");
            return false;
        }
    }
}