using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CityPvp : City3D
{
    public CityPvpSyn cityPvpSyn;
    public bool isSetupDone = false;
    public NetworkIdentity networkIdentity;

    public override void SetUp(int x, int y, int typeID, Team team)
    {
        if (!isSetupDone)
        {
            base.SetUp(x, y, typeID, team);
            if (networkIdentity.isServer)
            {
                cityPvpSyn.bCode = typeID;
                cityPvpSyn.x = x;
                cityPvpSyn.y = y;
                cityPvpSyn.team = team;
                cityPvpSyn.id = GameManager.inst.allCities.IndexOf(this);
            }
            isSetupDone = true;
        }
    }

    public override void OnArmyArrived(int armyPop, Team armyTeam)
    {
        if (!isDummy)
        {
            base.OnArmyArrived(armyPop, armyTeam);
        }
    }




}
