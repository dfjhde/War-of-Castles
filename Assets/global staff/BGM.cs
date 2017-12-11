using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour {


    public string IS_MUSIC_ON = "isMusicOn";
    public AudioSource[] audioSources;

    void Start ()
    {
        SetMusic(PlayerPrefs.GetInt(IS_MUSIC_ON, 1) == 1);
    }

    public void SetMusic(bool isOn)
    {
        foreach (AudioSource aus in audioSources)
        {
            aus.mute = !isOn;
        }
    }
}
