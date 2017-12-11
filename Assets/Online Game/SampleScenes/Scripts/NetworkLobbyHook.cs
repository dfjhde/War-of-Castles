using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        PvpPlayer pvpPlayer = gamePlayer.GetComponent<PvpPlayer>();

        //spaceship.name = lobby.name;
        pvpPlayer.team = (Team)lobby.playerTeamID;
        //spaceship.score = 0;
        //spaceship.lifeCount = 3;
    }
}
