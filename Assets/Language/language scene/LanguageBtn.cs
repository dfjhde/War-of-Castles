using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LanguageBtn : MonoBehaviour
{
    public Text text;
    public string key;
	// Use this for initialization
	public void SetUp (string key, string value, Transform parent)
    {
        this.transform.SetParent(parent);
        this.transform.SetSiblingIndex(0);
        text.text = value;
        this.key = key;
	}

    public void OnClick()
    {
        LanguageManager.inst.OnLanguageToggle(this.key);
    }
}
