using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CustomNetworkManager1 : NetworkManager
{
    public List<NetworkPlayer> networkPlayers = new List<NetworkPlayer>();
    [SerializeField] int maxGamePlayers;

    [Scene] string LobbyScene;
    [Scene] string gameScene;
    

    int minGamePlayers = 2;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (networkPlayers.Count > maxGamePlayers)
        {
            conn.Disconnect();
            return;
        }

        GameObject playerObj = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, playerObj);
        networkPlayers.Add(playerObj.GetComponent<NetworkPlayer>());

        StartGame();
    }

    
    public void StartGame()
    {
        if (networkPlayers.Count >= minGamePlayers)
        {
            ServerChangeScene(gameScene);
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (newSceneName == LobbyScene)
        {
            foreach (NetworkPlayer lobbyPlayer in networkPlayers)
            {
                if (lobbyPlayer == null)
                    continue;

                // find the game-player object for this connection, and destroy it
                NetworkIdentity identity = lobbyPlayer.GetComponent<NetworkIdentity>();

                if (NetworkServer.active)
                {
                    
                    NetworkServer.ReplacePlayerForConnection(identity.connectionToClient, lobbyPlayer.gameObject, ReplacePlayerOptions.KeepAuthority);
                }
            }

            //allPlayersReady = false;
        }
        base.ServerChangeScene(newSceneName);
    }


}

struct LobbyPlayer
{
    public NetworkConnectionToClient conn;
    public GameObject roomPlayer;
}