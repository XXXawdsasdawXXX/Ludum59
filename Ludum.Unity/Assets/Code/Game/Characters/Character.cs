using System;
using System.Collections.Generic;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using UnityEngine;

namespace Code.Game.Characters
{
    public abstract class Character : MonoBehaviour, IPoolEntity
    {
        public event Action<IPoolEntity> NeedToBeDisabled;
        public Dictionary<Type, ICharacterComponent> Components { get; } = new();

        public abstract void InitializeComponents();

        public T GetCharacterComponent<T>() where T : class, ICharacterComponent
        {
            Type type = typeof(T);

            if (Components.ContainsKey(type))
            {
                return Components[typeof(T)] as T;
            }

            Debug.LogError($"{gameObject.name} has not component with type {type.Name}", gameObject);

            return null;
        }
        
        public void Enable()
        {
            foreach (KeyValuePair<Type,ICharacterComponent> component in Components)
            {
                if (component.Value is IGameListener listener)
                {
                    Container.Instance.GetService<GameEventDispatcher>().AddRuntimeListener(listener);
                }
            }
        }

        public void Disable()
        {
            foreach (KeyValuePair<Type,ICharacterComponent> component in Components)
            {
                if (component.Value is IGameListener listener)
                {
                    Container.Instance.GetService<GameEventDispatcher>().RemoveRuntimeListener(listener);
                }
            }
        }

        protected void invokeNeedToBeDisabled()
        {
            NeedToBeDisabled?.Invoke(this);
        }
    }
}