using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArmyPvpSyn : NetworkBehaviour {

    [SyncVar]
    public Vector2 endPos = new Vector2(100f,100f);

    [SyncVar]
    public Vector2 startPos = new Vector2(-100f, -100f);

    [SyncVar]
    public int population = -1;

    [SyncVar]
    public Team team;

    public ArmyPvp armyPvp;

    // Use this for initialization
    void Start ()
    {
        if (!armyPvp.networkIdentity.isServer)
        {
            waitToSetUp();
        }

	}

    [ClientCallback]
    public void waitToSetUp()
    {
        StartCoroutine(setupClient());
    }

    IEnumerator setupClient()
    {
        yield return new WaitUntil(() => endPos != new Vector2(100f, 100f) && population != -1 && startPos != new Vector2(-100f, -100f));
        armyPvp.SetupClient(population, team, endPos, startPos);
    }
}
