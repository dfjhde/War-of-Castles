using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;

public class PvpMapManager : MonoBehaviour, MapSelector.ISelectMap {

    public static PvpMapManager inst;

    public MapSelector mapSelector;
    public MapBuilder mapBuilder;
    public Transform buildingListT;

    public LobbyPlayer localLobbyPlayer;

    void Start()
    {
        inst = this;
        
    }

    public void OnBtnChangeMap()
    {
        LobbyManager.s_Singleton.coverPanel.SetActive(true);
        mapSelector.gameObject.SetActive(true);
        mapSelector.SetUp();
    }

    public void OnBtnBack()
    {
        LobbyManager.s_Singleton.coverPanel.SetActive(false);
        mapSelector.gameObject.SetActive(false);
    }

    public void OnOtherPlayerSelectMap(string mapCode)
    {
        mapBuilder.BuildMap3D(mapCode, buildingListT);
        Data.inst.SetCurrentMap(new MapInfo(1,"pvpSelectedByOtherPlayer", mapCode), GameMode.PvP);
    }

    public void OnSelectMap(MapInfo mapInfo)
    {
        localLobbyPlayer.OnMapSeleted(mapInfo.mapCode);
        //Data.inst.SetCurrentMap(mapInfo, GameMode.PvP);
        //mapBuilder.BuildMap3D(mapInfo.mapCode, buildingListT);
        OnBtnBack();
    }

}
