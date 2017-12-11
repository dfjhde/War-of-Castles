using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArmyPvp : Army3D
{
    public ArmyPvpSyn armyPvpSyn;
    public NetworkIdentity networkIdentity;


    public override void Setup(int population, Team team, City from, City to)
    {
        if (networkIdentity.isServer)
        {
            base.Setup(population, team, from, to);
            armyPvpSyn.population = population;
            armyPvpSyn.endPos = endPos;
            armyPvpSyn.startPos = startPos;
            armyPvpSyn.team = team;
        }
        else if (networkIdentity.isClient)
        {
            PvpGameManager.inst.allArmies.Add(this);
        }
    }

    protected override bool IsArrived()
    {
        if (networkIdentity.isServer)
        {
            return base.IsArrived();
        }
        else { return false; }
    }

    protected override void DestroyArmy()
    {
        NetworkServer.Destroy(gameObject);
    }

    public void SetupClient(int pop, Team team, Vector2 endPos, Vector3 startPos)
    {
        this.endPos = endPos;
        this.startPos = startPos;
        SetPopulation(pop);
        SetTeam(team);
        transform.LookAt(new Vector3(endPos.x, this.transform.position.y, endPos.y));

    }
}
