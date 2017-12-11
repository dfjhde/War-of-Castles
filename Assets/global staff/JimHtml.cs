using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class JimHtml : MonoBehaviour
{

    public static void DownLoadText(MonoBehaviour obj, string url, Action<string, bool> processString)
    {
        obj.StartCoroutine(DownloadingText(url, processString));
    }

    private static IEnumerator DownloadingText(string url, Action<string, bool> processString)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        processString(www.downloadHandler.text, www.isNetworkError);
        /*
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            processString(www.downloadHandler.text, false);
        }
        else
        {
            processString(www.downloadHandler.text, true);
        }
        */
    }


    public static void DownLoadJson<T>(MonoBehaviour obj, string url, bool isArray, Action<T, bool> processT)
    {
        //obj.StartCoroutine(DownloadingJson(url, isArray, processT));
        DownLoadText(obj, url, (string json, bool isError)=> {
            if (isArray)
            {
                json = "{ \"list\": " + json + "}";
            }
            T t = JsonUtility.FromJson<T>(json);
            processT(t, isError);
        });
    }
}


