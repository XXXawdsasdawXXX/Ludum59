using System;
using System.Numerics;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Tools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Code.Game
{
    public class PlayerInput : IService, IStartListener, IUpdateListener
    {
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private const string VERTICAL_AXIS_NAME = "Vertical";
        private const KeyCode ACTION_KEY = KeyCode.Space;

        public Action UltPressed;
        public ReactiveProperty<Vector2> Forward { get; private set; }

        
        public UniTask GameStart()
        {
            Forward = new ReactiveProperty<Vector2>(Vector2.zero);
            
            return UniTask.CompletedTask;
        }

        public void GameUpdate()
        {
            Vector2 input = new Vector2(
                Input.GetAxisRaw(HORIZONTAL_AXIS_NAME), 
                Input.GetAxisRaw(VERTICAL_AXIS_NAME));

            Forward.PropertyValue = input;

            if (Input.GetKeyDown(ACTION_KEY))
            {
                UltPressed?.Invoke();
            }
        }
    }
}
