using FishNet.Connection;

namespace ProjectScare.ServiceLocator
{
    public interface IServiceNetworkSpawner : IService
    {
        void SpawnPlayer(NetworkConnection ownerConnection);
    }
}