using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;



public class MapSelector : MonoBehaviour
{
    public MonoBehaviour iSelectMap;
    public static MapSelector inst;
    public SelectMapBtn[] mapBtn;
    public Toggle toggleLatest;
    public Toggle toggleMyMap;
    public MapBuilder mapBuilder;

    public Text pageNumT;
    public InputField searchIdIF;
    public Sprite[] mapsIMGs;

    void Start()
    {
        inst = this;
        StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMapList.php?page=1"));
    }


    public void SetUp()
    {
        StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMapList.php?page=1"));
    }
    //public for NewChallengeManager.cs
    public IEnumerator GetText(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("download "+www.downloadHandler.text);
            unfoldString(www.downloadHandler.text);
        }
    }


    private void unfoldString(string s)
    {
        Debug.Log(s);
        //if there is only one map
        if (s[0] != '[')
        {
            s = "[ " + s + "]";
        }
        s = "{ \"list\": " + s + "}";
        
        Tasks tasks = JsonUtility.FromJson<Tasks>(s);
        Debug.Log(tasks.list.Length);

        for(int i=0; i < tasks.list.Length; i++)
        {
            Debug.Log(tasks.list[i].GetMapCode());
            mapBuilder.BuildMap2D(tasks.list[i].GetMapCode(), mapBtn[i].transform);
            mapBtn[i].SetUp(tasks.list[i]);
        }
    }


    public void OnBtnNext()
    {
        CleanUp();
        pageNumT.text = (int.Parse(pageNumT.text) + 1).ToString();
        StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMapList.php?page=" + pageNumT.text));
    }

    public void OnBtnPrevious()
    {
        int a = int.Parse(pageNumT.text);
        if (a <= 1) { return; }
        CleanUp();
        pageNumT.text = (a - 1).ToString();
        StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMapList.php?page=" + pageNumT.text));
    }

    public void OnBtnSearch()
    {
        CleanUp();
        StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMap.php?id=" + searchIdIF.text));
    }


    public void CleanUp()
    {
        foreach(SelectMapBtn btn in mapBtn)
        {
            btn.Hide();
        }
    }

    public void OnBtnBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }


    public void OnBtnMap(MapInfo mapInfo)
    {
        ((ISelectMap)iSelectMap).OnSelectMap(mapInfo);
    }

    public interface ISelectMap
    {
        void OnSelectMap(MapInfo mapInfo);
    }
}

[Serializable]
public class Tasks
{
    public MapInfo[] list;
}

/******** old stuff**********
 void Start()
    {
        string challengePanel = PlayerPrefs.GetString("challengePanel");
        
        if (challengePanel.Equals("latest"))
        {
            toggleLatest.isOn = true;
        }
        else if (challengePanel.Equals("myMap"))
        {
            toggleMyMap.isOn = true;
        }
        else
        {
            PlayerPrefs.SetString("challengePanel", "latest");
            Start();
        }
    }


     public void OnToggleLatest(bool isOn)
    {
        if (isOn)
        {
            CleanUp();
            StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMapList.php?page=1"));
        }

    }

    public void OnToggleHottest(bool isOn)
    {
        if (isOn)
        {
            CleanUp();
            StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMapList.php?page=1"));
        }
    }

    public void OnToggleMyMap(bool isOn)
    {
        if (isOn)
        {
            CleanUp();
            StartCoroutine(GetText("http://madebyjimchen.com/WarOfCastles/api/getMyMaps.php?userName=" + "JimChen" + "&page=2"));
        }
    }


*/


