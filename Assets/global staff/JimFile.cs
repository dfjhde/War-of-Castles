using UnityEngine;
using System.Collections;
using System.IO;

public enum StoreType { StreamingAssets, PlayerPrefs }

public class JimFile
{
    public static T LoadData<T>(string fileName)
    {
        string dataText;
#if UNITY_ANDROID
        dataText = LoadText(fileName, StoreType.PlayerPrefs);
#elif UNITY_IOS
        dataText = LoadText(fileName, StoreType.StreamingAssets);
#else
        dataText = LoadText(fileName, StoreType.StreamingAssets);
#endif
        return JsonUtility.FromJson<T>(dataText);
    }

    public static void SaveData<T>(string fileName, T Data)
    {
        string dataText = JsonUtility.ToJson(Data);

#if UNITY_ANDROID
        JimFile.SaveText(fileName, dataText, StoreType.PlayerPrefs);
#elif UNITY_IOS
        JimFile.SaveText(fileName, dataText, StoreType.StreamingAssets);
#elif UNITY_EDITOR
        JimFile.SaveText(fileName, dataText, StoreType.StreamingAssets);
#endif
    }

    public static string LoadTextFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataText = "";
        if (File.Exists(filePath))
        {
            dataText = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("Cannot find file: " + filePath);
        }
        return dataText;
    }

    public static void SaveTextToFile(string fileName, string text)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!string.IsNullOrEmpty(filePath))
        {
            File.WriteAllText(filePath, text);
        }
    }

    public static void SaveText(string fileName, string text, StoreType type)
    {
        switch (type)
        {
            case StoreType.StreamingAssets:
                SaveTextToFile(fileName, text);
                break;
            case StoreType.PlayerPrefs:
                PlayerPrefs.SetString(fileName, text);
                break;
        }
    }

    public static string LoadText(string fileName, StoreType type)
    {
        string dataText = "";
        switch (type)
        {
            case StoreType.StreamingAssets:
                dataText = LoadTextFromFile(fileName);
                break;
            case StoreType.PlayerPrefs:
                dataText = PlayerPrefs.GetString(fileName);
                break;
        }
        return dataText;
    }
}
