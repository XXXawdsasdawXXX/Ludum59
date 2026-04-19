using System;
using System.Linq;
using Code.Core.ServiceLocator;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace Code.Core.GameLoop
{
    [Preserve]
    public static class Spawner 
    {
        private static readonly Lazy<GameEventDispatcher> _lazyDispatcher =
            new(() => Container.Instance.GetService<GameEventDispatcher>());

        private static GameEventDispatcher _gameEventDispatcher => _lazyDispatcher.Value;
        
        
        public static T Instantiate<T>(T prefab) where T : MonoBehaviour
        {
            T instance = Object.Instantiate(prefab);

            if (instance == null)
            {
                return instance;
            }
            
            IGameListener[] listeners = instance.GetComponentsInChildren<IGameListener>(true).ToArray();

            foreach (IGameListener listener in listeners)
            {
                _gameEventDispatcher.AddRuntimeListener(listener);
            }

            return instance;
        }

        public static T Instantiate<T>(T prefab, Transform root) where T : MonoBehaviour
        {
            T instance = Object.Instantiate(prefab, root);

            if (instance == null)
            {
                return instance;
            }
            
            return instance;
        }

        public static void ConnectToGameLoop(GameObject gameObject)
        {
            IGameListener[] listeners = gameObject.GetComponentsInChildren<IGameListener>(true).ToArray();

            foreach (IGameListener listener in listeners)
            {
                _gameEventDispatcher.AddRuntimeListener(listener);
            }
        }
        
        public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            T instance = Object.Instantiate(prefab, position, rotation);

            if (instance == null)
            {
                return instance;
            }

            IGameListener[] listeners = instance.GetComponentsInChildren<IGameListener>(true).ToArray();

            foreach (IGameListener listener in listeners)
            {
                _gameEventDispatcher.AddRuntimeListener(listener);
            }

            return instance;
        }

        public static void Destroy(GameObject instance)
        {
            IGameListener[] listeners = instance.GetComponentsInChildren<IGameListener>(true).ToArray();

            foreach (IGameListener listener in listeners)
            {
                _gameEventDispatcher.RemoveRuntimeListener(listener);
            }
            
            Object.Destroy(instance);
        }
    }
}