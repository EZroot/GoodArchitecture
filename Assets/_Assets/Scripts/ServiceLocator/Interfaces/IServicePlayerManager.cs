using System.Collections.Generic;

namespace ProjectScare.ServiceLocator
{
    public interface IServicePlayerManager : IService
    {
        event PlayerManager.OnPlayerClientAddDelegate OnPlayerClientAdded;
        event PlayerManager.OnPlayerClientStatsChangedDelegate OnPlayerClientStatsChanged;
        List<PlayerEntity> PlayerCollection {get;}
        ClientStats ClientStats {get;}
        void AddPlayer(PlayerEntity player);
        void RemovePlayer(PlayerEntity player);
        void SetClientStats(int clientId, string username, string connectionAddress);
        void TestFunc();
    }
}