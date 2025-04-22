using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : NetworkBehaviour
{
    [SerializeField] float timeOut;

    [SerializeField] TMP_Text textBox;

    bool isTimeOut;

    public static Timer instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(GameStartCountdown());
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    [Command(requiresAuthority = false)]
    public void CmdStartCountDown()
    {
        StartCountDown();
    }

    [ClientRpc]
    public void StartCountDown()
    {
        textBox.text = "Player joined";
        StartCoroutine(GameStartCountdown());
    }

    IEnumerator GameStartCountdown()
    {
       
        float timeLeft = timeOut;

        while (timeLeft > 0)
        {
            textBox.text = $"Game starts in {timeLeft} seconds...";
            //Debug.Log($"Game starts in {timeLeft} seconds...");
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        if (isServer && isClient)
        {
            CustomNetworkManager.singleton.LoadGameScene();
        }
    }

    public void CloseBtn()
    {
        FindAnyObjectByType<CustomNetworkHUD>().StopButton();
    }
}
