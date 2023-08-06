using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using FishNet;
using ProjectScare.ServiceLocator;

public class SynchronizeScene : NetworkBehaviour
{
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        if(base.IsHost)
        {
            base.GiveOwnership(base.NetworkManager.ServerManager.Clients[0]);
            RpcSpawnPlayers();
        }
    }

    [ServerRpc]
    private void RpcSpawnPlayers()
    {
        var network = ServiceLocator.Get<IServiceNetworkManager>();
        var spawner = ServiceLocator.Get<IServiceNetworkSpawner>();

        var playerCollection = network.CurrentConnectedPlayerStatsCollection;

        foreach (var con in playerCollection)
        {
            var connection = con.PlayerNetworkConnection;
            spawner.SpawnPlayer(connection);
        }
    }
}
