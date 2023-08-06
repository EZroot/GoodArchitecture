using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectScare.ServiceLocator;
using UnityEngine;
public class BootstrapScene : MonoBehaviour
{
    public async void Start()
    {
        await LoadGame();
    }

    async UniTask LoadGame()
    {
        var datamgr = ServiceLocator.Get<IServiceDataManager>();
        await datamgr.LoadAndProcessData();
        Debug.Log("Bootstrap: Data loaded success.");
        
        var sceneMgr = ServiceLocator.Get<IServiceSceneManager>();
        await sceneMgr.LoadSceneAddressableAsync(SceneManager.SceneType.LobbyScene);
    }
}
