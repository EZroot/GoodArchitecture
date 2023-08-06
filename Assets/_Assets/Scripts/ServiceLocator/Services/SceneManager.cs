using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace ProjectScare.ServiceLocator
{
    public class SceneManager : MonoBehaviour, IServiceSceneManager
    {

        public enum SceneType
        {
            BootstrapScene,
            LobbyScene,
            CabinScene
        }

        public delegate void OnSceneChangeDelegate();
        public event OnSceneChangeDelegate OnSceneFinishedLoading;

        [SerializeField] private SceneSettingsScriptableObject _sceneSettingsSO;
        [Header("Tip: This scene should also be loaded before hitting play.")]
        [SerializeField] private SceneType _firstScene;
        [SerializeField] private bool _loadFirstSceneOnPlay = false;

        SceneInstance _currentActiveSceneInstance;


        async void Start()
        {
            await Initialize();
        }

        async UniTask Initialize()
        {
            if (_loadFirstSceneOnPlay)
            {
                await LoadSceneAddressableAsync(_firstScene);
            }
        }

        public async UniTask LoadSceneAddressableAsync(SceneType sceneType)
        {
            string sceneName = sceneType switch
            {
                SceneType.BootstrapScene => _sceneSettingsSO.BootstrapSceneName,
                SceneType.LobbyScene => _sceneSettingsSO.LobbySceneName,
                SceneType.CabinScene => _sceneSettingsSO.CabinSceneName,
                _ => "<color=red>SCENE TYPE DOESNT EXIST</color>"
            };

            if (_currentActiveSceneInstance.Scene.isLoaded)
            {
                var unloadResult = await Addressables.UnloadSceneAsync(_currentActiveSceneInstance, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
            _currentActiveSceneInstance = await Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if(OnSceneFinishedLoading != null)
            {
                OnSceneFinishedLoading();
            }
        }
    }
}