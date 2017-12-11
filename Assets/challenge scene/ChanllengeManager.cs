using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanllengeManager : MonoBehaviour, MapSelector.ISelectMap {


    public void OnSelectMap(MapInfo mapInfo)
    {
        Data.inst.SetCurrentMap(mapInfo, GameMode.Challenge);
        UnityEngine.SceneManagement.SceneManager.LoadScene("game3D");
    }

    public void OnBtnBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("online");
    }
}
