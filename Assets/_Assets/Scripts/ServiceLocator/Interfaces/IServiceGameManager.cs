namespace ProjectScare.ServiceLocator
{
    public interface IServiceGameManager : IService
    {
        GameSettings GameSettings { get; }
    }
}