using Mirror;
using TMPro;
using UnityEngine;

public class CustomNetworkHUD : MonoBehaviour
{
    [SerializeField] TMP_InputField addressInputField;
    NetworkManager manager;

    public void Start()
    {
        manager = GetComponent<NetworkManager>();
        addressInputField.text = "localhost";
    }


    public void Host()
    {
        manager.StartHost();
    }

    public void Client()
    {
        manager.networkAddress = addressInputField.text;
        manager.StartClient();
    }
    public void StopButton()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
    }
}
