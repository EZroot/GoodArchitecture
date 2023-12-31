using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using FishNet.Connection;

namespace ProjectScare.ServiceLocator
{
    public class PlayerManager : MonoBehaviour, IServicePlayerManager
    {
        public delegate void OnPlayerClientAddDelegate(PlayerEntity playerEntity);
        public event OnPlayerClientAddDelegate OnPlayerClientAdded;
        public delegate void OnPlayerClientStatsChangedDelegate(ClientStats clientStats);
        public event OnPlayerClientStatsChangedDelegate OnPlayerClientStatsChanged;

        [ShowInInspector] private List<PlayerEntity> _playerCollection;
        [ShowInInspector] private ClientStats _clientStats;

        public List<PlayerEntity> PlayerCollection => _playerCollection;
        public ClientStats ClientStats => _clientStats;

        void Start()
        {
            _playerCollection = new List<PlayerEntity>();
        }

        public void AddPlayer(PlayerEntity playerEntity)
        {
            _playerCollection.Add(playerEntity);
            if(OnPlayerClientAdded != null)
                OnPlayerClientAdded(playerEntity);
        }

        public void RemovePlayer(PlayerEntity playerEntity)
        {
            var entityFound = false;
            for (var i = 0; i < _playerCollection.Count; i++)
            {
                if (_playerCollection[i].EntityData.ID == playerEntity.EntityData.ID)
                {
                    _playerCollection.RemoveAt(i);
                    break;
                }
            }

            if (entityFound == false)
            {
                _playerCollection.Add(playerEntity);
            }
        }

        public void TestFunc()
        {
            Debug.Log("Player Manager called");
        }

        public void SetClientStats(int clientId, string username, string connectionAddress)
        {
            _clientStats = new ClientStats(clientId, username, connectionAddress);
            if(OnPlayerClientStatsChanged != null)
                OnPlayerClientStatsChanged(_clientStats);
        }
    }
}