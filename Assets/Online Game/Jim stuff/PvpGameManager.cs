using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.UI;

public class PvpGameManager : GameManager3D
{
    public static PvpGameManager instPvp;
    public NetworkIdentity networkIdentity;
    public PvpPlayer localPvpPlayer;
    public PvpGameManagerSyn pvpGameManagerSyn;
    public Button btnBackToLobby;

    public Button btnDisconnect;
    public JimText disconnectOverBtnT;
    public JimText disconnectStopBtnT;

    public string EXIT_ROOM = "Exit Room";
    public string CLOSE_ROOM = "Close Room";
    void Awake()
    {
        inst = this;
        instPvp = this;
        pvpGameManagerSyn.isPlaying = false;
    }

    void Start()
    {
        //to override
        disconnectOverBtnT.SetText(networkIdentity.isServer ? CLOSE_ROOM : EXIT_ROOM);
        disconnectStopBtnT.SetText(networkIdentity.isServer ? CLOSE_ROOM : EXIT_ROOM);
    }

    public void onStart()
    {
        if (networkIdentity.isServer)
        {
            OnStartGame();
        }

        if (networkIdentity.isClient)
        {
            InstEnvironment();
        }
    }

    protected override void SetIsPlaying(bool isPlaying)
    {
        base.SetIsPlaying(isPlaying);
        pvpGameManagerSyn.isPlaying = isPlaying;
    }


    public override Army InstantiateArmy(City fromCity)
    {
        Army army = base.InstantiateArmy(fromCity);
        NetworkServer.Spawn(army.gameObject);
        return army;
    }

    public override City InstantiateCityObj(BuildingInfo buildingInfo)
    {
        City city = base.InstantiateCityObj(buildingInfo);
        NetworkServer.Spawn(city.gameObject);
        return city;
    }

    public override void BuildFroce(List<City> fromCities, City targetCity)
    {
        if (networkIdentity.isServer)
        {
            base.BuildFroce(fromCities, targetCity);
        }
        else if (networkIdentity.isClient)
        {
            int[] fromIDs = new int[fromCities.Count];
            for (int i = 0; i <fromCities.Count; i++)
            {
                fromIDs[i] = ((CityPvp)fromCities[i]).cityPvpSyn.id;
            }

            int toID = ((CityPvp)targetCity).cityPvpSyn.id;
            Debug.Log("ClientBuildFroce");
            localPvpPlayer.callCmdBuildFroce(fromIDs, toID);
        }
    }

    public void OnBtnStop()
    {
        localPvpPlayer.callCmdStop();
    }

    public void OnBtnContinue()
    {
        localPvpPlayer.callCmdContinue();
    }

    public void OnBtnBackToLobby()
    {
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }

    public void OnBtnDisconnect()
    {
        LobbyManager.s_Singleton.backDelegate();
    }

    public void OnBtnShotDownServer()
    {
        LobbyManager.s_Singleton.backDelegate();
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        
    }
}
