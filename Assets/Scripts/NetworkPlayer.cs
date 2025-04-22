using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    [SyncVar]
    public int foldOrContest;

    [SyncVar]
    public int number; 

    [HideInInspector]
    public GameManager gameManager;
    public override void OnStartLocalPlayer()
    {
        playerName = "Player " + Random.Range(0, 100);
        CmdSetName(playerName);
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene == "Game Scene")
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gameManager.localPlayer = this;
        }
    }

    [Command]
    void CmdSetName(string name)
    {
        playerName = name;
    }

    [Command]
    public void SetFoldOrContest(int New)
    {
        foldOrContest = New;
    }

    [Command]
    public void SetRandomNumber(int New)
    {
        number = New;
    }
}
