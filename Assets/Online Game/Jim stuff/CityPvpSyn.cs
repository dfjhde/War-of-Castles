using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CityPvpSyn : NetworkBehaviour {

    public City3D city3D;

    [SyncVar]
    public int y = -1;

    [SyncVar]
    public int x = -1;

    [SyncVar]
    public int bCode = -1;

    [SyncVar]
    public int id = -1;

    [SyncVar(hook = "OnPopulationChanged")]
    public int population;

    [SyncVar(hook = "OnTeamChanged")]
    public Team team;

    void Start ()
    {
        city3D.isDummy = Network.isClient;
        //city3D.SetUp();
        waitToSetUp();
	}

    [ClientCallback]
    public void waitToSetUp()
    {
        StartCoroutine(setupClient());
    }

    IEnumerator setupClient()
    {
        yield return new WaitUntil(() => x != -1 && y != -1 && bCode != -1 && id != -1);
        city3D.SetUp(x, y, bCode, team);
        GameManager.inst.allCities.Insert(id, city3D);
    }

    [ServerCallback]
    private void Update()
    {
        if (city3D.GetPopulation() != population)
        {
            population = city3D.GetPopulation();
        }

        if (city3D.GetTeam() != team)
        {
            team = city3D.GetTeam();
        }
    }

    private void OnbCode(int code)
    {
        Debug.Log("c");
        bCode = code;
        city3D.meshFilter.mesh = Data.inst.cityInfos[code].mesh;
    }

    private void OnPopulationChanged(int pop)
    {
        population = pop;
        city3D.SetPopulation(pop);

    }

    private void OnTeamChanged(Team newTeam)
    {
        team = newTeam;
        city3D.SetTeam(newTeam);

    }


    [ContextMenu("Do Something")]
    void DoSomething()
    {
        city3D.SetPopulation(100);
    }
}
