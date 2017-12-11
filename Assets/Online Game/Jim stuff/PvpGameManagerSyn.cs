using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PvpGameManagerSyn : NetworkBehaviour {

    [SyncVar(hook ="OnIsPlayingChanged")]
    public bool isPlaying = false;

    public PvpGameManager pvpGameManager;

    [ClientCallback]
    public void Start()
    {
        StartCoroutine(waitForStart());
    }

    IEnumerator waitForStart()
    {
        yield return new WaitUntil(() => isPlaying);
        pvpGameManager.isPlaying = isPlaying;
    }

    public void OnIsPlayingChanged(bool newState)
    {
        isPlaying = newState;
        pvpGameManager.isPlaying = newState;
        if (isPlaying) { pvpGameManager.Continue(); }
        else { pvpGameManager.Stop(); }
    }

}
