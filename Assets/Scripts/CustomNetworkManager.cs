using UnityEngine;
using Mirror;
using System.Collections.Generic;
using Mirror.Examples.NetworkRoom;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public List<NetworkPlayer> networkPlayers = new List<NetworkPlayer>();
    [SerializeField] int maxGamePlayers;

    
    [SerializeField]string gameScene;

    public static new CustomNetworkManager singleton => NetworkManager.singleton as CustomNetworkManager;

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
        if (networkPlayers.Count >= minGamePlayers)
        {
            if(SceneManager.GetActiveScene().name == "Lobby Scene")
            {
                Timer.instance.CmdStartCountDown();
            }
        }
    }


    public void LoadGameScene()
    {
        ServerChangeScene(gameScene);
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        networkPlayers.Clear();
        base.OnServerChangeScene(newSceneName);
    }
}