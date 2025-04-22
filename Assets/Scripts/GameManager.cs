using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] TMP_Text textBox,countDownText,popupText,playerNameText;

    int countDown = 10;

    public NetworkPlayer localPlayer;

    [SerializeField] GameObject popUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        CmdStartGame();
    }


    [Command(requiresAuthority = false)]
    void CmdStartGame()
    {
        StartGame();
    }


    [ClientRpc]
    void StartGame()
    {
        int randomNumber = Random.Range(1, 101);
        textBox.text = randomNumber.ToString();
        localPlayer.number = randomNumber;
        localPlayer.SetRandomNumber(randomNumber);
        StartCoroutine(GameStartCountdown());
        playerNameText.text = localPlayer.playerName;
    }

    IEnumerator GameStartCountdown()
    {

        float timeLeft = countDown;

        while (timeLeft >= 0)
        {
            SetCountDown(timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        if (isServer && isClient)
        {
            CmdCheckResult();
        }
    }

    void SetCountDown(float countDown)
    {
        countDownText.text = countDown.ToString();
    }

    [Command(requiresAuthority = false)]
    void CmdCheckResult()
    {
        NetworkPlayer winPlayer = GetWinPlayer();
        Result(winPlayer.playerName);
    }


    NetworkPlayer GetWinPlayer()
    {
        List<NetworkPlayer> list = new List<NetworkPlayer>();
        for (int i = 0; i < CustomNetworkManager.singleton.networkPlayers.Count; i++) 
        {
            if (CustomNetworkManager.singleton.networkPlayers[i].foldOrContest == 1)
            {
                list.Add(CustomNetworkManager.singleton.networkPlayers[i]);
            }
        }
       
        return CheckHighestNumber(list);
    }

    NetworkPlayer CheckHighestNumber(List<NetworkPlayer> playerlist)
    {
        NetworkPlayer networkPlayer = null;
        for (int i = 0; i < playerlist.Count; i++) 
        {
            NetworkPlayer player = playerlist[i];
            for (int j = 0; j < playerlist.Count; j++) 
            {
                if(player.number >= playerlist[j].number)
                {
                    networkPlayer = player;
                }
            }
        }
        return networkPlayer;
    }

    [ClientRpc]
    void Result(string playerName)
    {
        StartCoroutine(ShowPopUp(playerName));
    }

    IEnumerator ShowPopUp(string PlayerName)
    {
        popupText.text = PlayerName;
        popUp.SetActive(true);
        yield return new WaitForSeconds(6f);
        popUp.SetActive(false);

        InitializeGame();
    }



    public void FoldBtnFn()
    {
        localPlayer.foldOrContest = 0;
        localPlayer.SetFoldOrContest(0);
    }

    public void ContestBtnFn()
    {
        localPlayer.foldOrContest = 1;
        localPlayer.SetFoldOrContest(1);
    }


}
