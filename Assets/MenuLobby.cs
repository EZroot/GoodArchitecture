using System.Collections;
using System.Collections.Generic;
using ProjectScare.ServiceLocator;
using UnityEngine;
using TMPro;
using FishNet.Connection;
using FishNet.Serializing;
using FishNet.Observing;
using FishNet.Object;
using FishNet.Transporting;

public class MenuLobby : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField;

    void Start()
    {
        var netwrk = ServiceLocator.Get<IServiceNetworkManager>();
        netwrk.FishnetManager.ClientManager.OnClientConnectionState += OnConnectedClient;
    }
    public void StartServer()
    {
        var netwrk = ServiceLocator.Get<IServiceNetworkManager>();
        netwrk.StartOrStopServer();
    }

    public void StartClient()
    {
        var netwrk = ServiceLocator.Get<IServiceNetworkManager>();
        netwrk.StartOrStopClient();

    }

    void OnConnectedClient(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            var netwrk = ServiceLocator.Get<IServiceNetworkManager>();
            var playerMgr = ServiceLocator.Get<IServicePlayerManager>();
            var connection = netwrk.FishnetManager.ClientManager.Connection;
            var clientId = connection.ClientId;
            var address = connection.GetAddress();
            var username = _usernameInputField.text;
            Debug.Log($"OnConnectedClient: {clientId} {address}");
            playerMgr.SetClientStats(clientId, username, address);
        }
    }
}
