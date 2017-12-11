using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class JimText : MonoBehaviour
{
    public Text UItext;


    void Start ()
    {
	    this.UItext.text = Data.inst.language.GetLanguage(this.UItext.text);
    }

    public void SetText(string textEng)
    {
        this.UItext.text = Data.inst.language.GetLanguage(textEng);
    }

}


[Serializable]
public class Language
{
    public string fileName = "language.txt";
	public LanguageSource languageSource;

    public Language(){}

    public void SetUp()
    {
        //DownloadLanguageSource();
        LoadLanguageSource();
    }

    public void SetLanguageName(string languageName, Action<bool> after)
    {
        if (languageName != GetLanguageName())
        {
            PlayerPrefs.SetString("language", languageName);
            string url = "http://madebyjimchen.com/WarOfCastles/api/getLanguageSource.php?language=" + languageName;

            JimHtml.DownLoadJson<LanguageSource>(Data.inst, url, false, (LanguageSource lang, bool isError) =>
            {
                if (!isError)
                {
                    this.languageSource = lang;
                    SaveLanguageSource();
                }
                after(isError);
            });
        }
        else
        {
            after(true);
        }
    }

    public string[] GetLanguageList()
    {
        return new string[3];
    }

    public string GetLanguageName()
    {
        return PlayerPrefs.GetString("language", "English");
    }

    public string GetLanguage(string key)
    {
		if(languageSource.keys.Contains(key))
		{
			int index = languageSource.keys.IndexOf(key);
			return languageSource.values[index];
		}
        else 
		{
			return key; 
		}
    }

	public void processLang(LanguageSource lang)
	{
		this.languageSource = lang;
		SaveLanguageSource();
	}

    private void LoadLanguageSource()
    {
        this.languageSource = JimFile.LoadData<LanguageSource>(fileName);
        if (languageSource == null)
        {
            languageSource = new LanguageSource();
        }
    }

    private void SaveLanguageSource()
    {
        JimFile.SaveData<LanguageSource>(fileName, this.languageSource);
    }
}
	

[Serializable]
public class LanguageSource
{
	public List<string> keys;
	public string[] values;

    public LanguageSource()
    {
        keys = new List<string>();
        values = new string[keys.Count];
    }
}