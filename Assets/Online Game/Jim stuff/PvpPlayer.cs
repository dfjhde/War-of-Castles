using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PvpPlayer : NetworkBehaviour
{

    public NetworkIdentity networkIdentity;

    [SyncVar]
    public Team team;

    //-------------------inst--------------------------------
    public void Start()
    {
        Debug.Log(networkIdentity.isLocalPlayer);
        if (networkIdentity.isLocalPlayer)
        {
            StartCoroutine(FindManager());
        }
    }

    IEnumerator FindManager()
    {
        yield return new WaitUntil(() => GameManager.inst != null);
        GameManager.inst.playerTeam = team;
        PvpGameManager.instPvp.localPvpPlayer = this;
        PvpGameManager.instPvp.onStart();
    }


    //----------------------------BuildFroce-----------------------------
    public void callCmdBuildFroce(int[] fromIDs, int targetCityID)
    {
        Debug.Log("callCmdBuildFroce");
        CmdBuildFroce(fromIDs, targetCityID);
    }


    [Command]
    public void CmdBuildFroce(int[] fromIDs, int targetCityID)
    {
        Debug.Log("CmdBuildFroce");
        if (networkIdentity.isServer) //avoid to create bullet twice (here & in Rpc call) on hosting client
        {
            List<City> fromCities = new List<City>();

            for (int i = 0; i < fromIDs.Length; i++)
            {
                fromCities.Add(GameManager.inst.allCities[fromIDs[i]]);
            }

            City targetCity = GameManager.inst.allCities[targetCityID];
            GameManager3D.inst.BuildFroce(fromCities, targetCity);
        }
    }


    //-----------------------------stop-----------------------------------
    public void callCmdStop()
    {
        CmdStop();
    }

    [Command]
    public void CmdStop()
    {
        GameManager.inst.Stop();
    }

    //----------------------------continue-------------------------------
    public void callCmdContinue()
    {
        CmdContinue();
    }

    [Command]
    public void CmdContinue()
    {
        GameManager.inst.Continue();
    }

    /*-------------------------game over-------------------------------------
    public void callCmdWin()
    {
        CmdWin();
    }

    [Command]
    public void CmdWin()
    {
        PvpGameManager.instPvp.gameOverCount--;
        if (PvpGameManager.instPvp.gameOverCount <= 0)
        {
            RpcGameOver();
        }
    }

    [ClientRpc]
    public void RpcGameOver()
    {
        PvpGameManager.instPvp.btnBackToLobby.gameObject.SetActive(true);
    }
    */

}