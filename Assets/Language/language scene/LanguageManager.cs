using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager inst;
    public ToggleGroup toggleGroup;
    public GameObject togglePre;
    public Text loadingText;
    public RectTransform content;
    
    public LanguageSource source;

	void Start ()
    {
        inst = this;
        
        loadingText.gameObject.SetActive(true);
        content.gameObject.SetActive(false);
        string url = "http://madebyjimchen.com/WarOfCastles/api/getLanguageList.php";
        JimHtml.DownLoadJson<LanguageSource>(Data.inst, url, false, processList);
    }


    private void processList(LanguageSource source, bool isError)
    {
        if (isError)
        {
            loadingText.text = "couldn't connect to the server";
            return;
        }
        this.source = source;

        for(int i = 1; i < source.keys.Count; i++)//the first one is id, which is not necessary
        {
            GameObject g = Instantiate(togglePre) as GameObject;
            g.GetComponent<LanguageBtn>().SetUp(source.keys[i], source.values[i], content);
        }
        loadingText.gameObject.SetActive(false);
        content.gameObject.SetActive(true);
    }

    public void OnLanguageToggle(string key)
    {
        Data.inst.language.SetLanguageName(key, after);
        loadingText.gameObject.SetActive(true);
        content.gameObject.SetActive(false);
    }

    public void after(bool isError)
    {
        if (isError) { loadingText.text = "couldn't connect to the server"; }
        else { backToTitle(); }
    }

    public void backToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }


}

