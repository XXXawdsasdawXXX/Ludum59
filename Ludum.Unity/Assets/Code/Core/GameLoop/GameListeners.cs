using Cysharp.Threading.Tasks;

namespace Code.Core.GameLoop
{
    public interface IGameListener
    {
    }

    public interface ISubscriber : IGameListener
    {
        void Subscribe();
        void Unsubscribe();
    }
    
    public  interface IInitializeListener : IGameListener
    {
        UniTask GameInitialize();
    }

    public interface ILoadListener : IGameListener
    {
        UniTask GameLoad();
    }

    public interface IStartListener : IGameListener
    {
        UniTask GameStart();
    }

    public interface IUpdateListener : IGameListener
    {
        void GameUpdate();
    }
    
    public interface IFixedUpdateListener : IGameListener
    {
        void GameFixedUpdate();
    }
    
    public interface IExitListener : IGameListener
    {
        void GameExit();
    }
}