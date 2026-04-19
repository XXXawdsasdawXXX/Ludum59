using System.Collections.Generic;
using Code.Core.ServiceLocator;
using Cysharp.Threading.Tasks;
//using Kirurobo;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;

namespace Code.Core.GameLoop
{
    public class GameEventDispatcher : MonoBehaviour, IService
    {
        private enum State
        {
            None,
            Initialize,
            Subscribe,
            Load,
            Start,
            Update,
            Unsubscribe,
            Exit
        }

        private readonly HashSet<IInitializeListener> _initListeners = new HashSet<IInitializeListener>();
        private readonly HashSet<ILoadListener> _loadListeners = new HashSet<ILoadListener>();
        private readonly HashSet<IStartListener> _startListeners = new HashSet<IStartListener>();
        private readonly HashSet<IUpdateListener> _updateListeners = new HashSet<IUpdateListener>();
        private readonly HashSet<IFixedUpdateListener> _fixedUpdateListeners = new HashSet<IFixedUpdateListener>();
        private readonly HashSet<IExitListener> _exitListeners = new HashSet<IExitListener>();
        private readonly HashSet<ISubscriber> _subscribers = new HashSet<ISubscriber>();

        private State _currentState;


        private void Awake()
        {
            _initializeListeners();
            _bootGame();
        }

        private async void Start()
        {
            await UniTask.WaitUntil(() => _currentState is State.Start);

            await _notifyGameStart();

            _currentState = State.Update;
        }

        private void Update()
        {
            if (_currentState is State.Update)
            {
                _notifyGameUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (_currentState is State.Update)
            {
                _notifyGameFixedUpdate();
            }
        }

        private void OnApplicationQuit()
        {
            if (_currentState > State.Subscribe)
            {
                _notifyGameExit();
            }
        }

        public async void AddRuntimeListener(IGameListener listener)
        {
            if (listener is IInitializeListener initListener)
            {
                await initListener.GameInitialize();
            }

            if (listener is ISubscriber subscriber && _subscribers.Add(subscriber))
            {
                subscriber.Subscribe();
            }

            if (listener is ILoadListener loadListener && _loadListeners.Add(loadListener))
            {
                await loadListener.GameLoad();
            }

            if (listener is IStartListener startListener && _startListeners.Add(startListener))
            {
                await startListener.GameStart();
            }

            if (listener is IUpdateListener updateListener)
            {
                _updateListeners.Add(updateListener);
            }

            if (listener is IFixedUpdateListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Add(fixedUpdateListener);
            }

            if (listener is IExitListener exitListener)
            {
                _exitListeners.Add(exitListener);
            }
        }

        public void RemoveRuntimeListener(IGameListener listener)
        {
            if (listener is IUpdateListener updateListener)
            {
                _updateListeners.Remove(updateListener);
            }

            if (listener is IFixedUpdateListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Remove(fixedUpdateListener);
            }

            if (listener is ISubscriber subscriber)
            {
                subscriber.Unsubscribe();

                _subscribers.Remove(subscriber);
            }

            if (listener is IExitListener exitListener)
            {
                exitListener.GameExit();
                
                _exitListeners.Remove(exitListener);
            }
        }

        private async void _bootGame()
        {
            _currentState = State.Initialize;
            await _notifyGameInitialize();

            _currentState = State.Subscribe;
            await _notifySubscribe();

            _currentState = State.Load;
            await _notifyGameLoad();

            _currentState = State.Start;
        }

        private void _initializeListeners()
        {
            List<IGameListener> gameListeners = Container.Instance.GetGameListeners();

            foreach (IGameListener listener in gameListeners)
            {
                if (listener is IInitializeListener initListener) _initListeners.Add(initListener);

                if (listener is ISubscriber subscriber) _subscribers.Add(subscriber);

                if (listener is ILoadListener loadListener) _loadListeners.Add(loadListener);

                if (listener is IStartListener startListener) _startListeners.Add(startListener);

                if (listener is IUpdateListener updateListener) _updateListeners.Add(updateListener);

                if (listener is IFixedUpdateListener fixedUpdateListener)
                    _fixedUpdateListeners.Add(fixedUpdateListener);

                if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);
            }
        }

        private async UniTask _notifyGameInitialize()
        {
            foreach (IInitializeListener listener in _initListeners)
            {
                await listener.GameInitialize();
            }
        }

        private async UniTask _notifyGameLoad()
        {
            foreach (ILoadListener listener in _loadListeners)
            {
                await listener.GameLoad();
            }
        }

        private UniTask _notifySubscribe()
        {
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Subscribe();
            }

            return UniTask.CompletedTask;
        }

        private async UniTask _notifyGameStart()
        {
            foreach (IStartListener listener in _startListeners)
            {
                await listener.GameStart();
            }
        }
        
        private void _notifyGameUpdate()
        {
            List<IUpdateListener> list = new(_updateListeners);

            foreach (IUpdateListener listener in list)
            {
                listener.GameUpdate();
            }
        }

        private void _notifyGameFixedUpdate()
        {
            foreach (IFixedUpdateListener listener in _fixedUpdateListeners)
            {
                listener.GameFixedUpdate();
            }
        }

        private void _notifyGameExit()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_notifyGameExit");
            marker.Begin();
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }

            foreach (IExitListener listener in _exitListeners)
            {
                listener.GameExit();
            }

            marker.End();
#else
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }

            foreach (IExitListener listener in _exitListeners)
            {
                listener.GameExit();
            }
#endif
        }
    }
}