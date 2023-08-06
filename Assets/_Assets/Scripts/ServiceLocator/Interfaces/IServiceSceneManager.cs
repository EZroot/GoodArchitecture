using Cysharp.Threading.Tasks;

namespace ProjectScare.ServiceLocator
{
    public interface IServiceSceneManager : IService
    {
        event SceneManager.OnSceneChangeDelegate OnSceneFinishedLoading;
        UniTask LoadSceneAddressableAsync(SceneManager.SceneType sceneType);
    }
}