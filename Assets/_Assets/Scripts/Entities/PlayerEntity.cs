using FishNet.Connection;
using FishNet.Object;
using ProjectScare.ServiceLocator;
using UnityEngine;

public class PlayerEntity : Entity<PlayerEntityData>
{
    void Awake()
    {
                var sceneManager = ServiceLocator.Get<IServiceSceneManager>();
        
        sceneManager.OnSceneFinishedLoading += Initialize;
    }
    void Start()
    {
        var playerManager = ServiceLocator.Get<IServicePlayerManager>();

        playerManager.AddPlayer(this);

        OnEntityDataChanged += SaveEntityData;
    }
    void OnDestroy()
    {
        var playerManager = ServiceLocator.Get<IServicePlayerManager>();
        playerManager.RemovePlayer(this);
        OnEntityDataChanged -= SaveEntityData;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
    }

    public override void Initialize()
    {
        if (base.IsOwner)
        {
            base.Initialize();
            var netwrk = ServiceLocator.Get<IServiceNetworkManager>();
            var connection = netwrk.FishnetManager.ClientManager.Connection;

            var playerManager = ServiceLocator.Get<IServicePlayerManager>();
            playerManager.SetClientStats(connection.ClientId, playerManager.ClientStats.Username, connection.GetAddress());

            _entityData.SetClientStats(playerManager.ClientStats);

            RPCSetPlayerDataServer(_entityData);
            RPCSpawnEntity(connection);
        }
    }

    [ServerRpc]
    void RPCSetPlayerDataServer(PlayerEntityData data)
    {
        RPCSetPlayerDataClient(data);
    }

    [ObserversRpc(BufferLast = true)]
    void RPCSetPlayerDataClient(PlayerEntityData data)
    {
        SetEntityData(data);
    }

    [ServerRpc]
    void RPCSpawnEntity(NetworkConnection connection)
    {
        Debug.Log($"Spawned with connection: {connection.ClientId}");
        var spawner = ServiceLocator.Get<IServiceNetworkSpawner>();
        spawner.SpawnPlayer(connection);
    }
}
