using System;
using System.Security.Cryptography;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.World;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerAnimation : ICharacterComponent, IUpdateListener, ISubscriber
    {
        public Condition Condition { get; } = new Condition();
        
        private readonly Rigidbody2D _rigidBody;
        private readonly PlayerRender _render;

        private float _forward;
        private readonly PlayerView _view;
        private readonly MachineSpawner _machineSpawner;

        private MachineView _currentMachine;
        
        public PlayerAnimation(PlayerView view)
        {
            _rigidBody = view.Rigidbody2D;
            _render = view.Renderer;
            _view = view;
            _machineSpawner = Container.Instance.GetService<MachineSpawner>();
        }

        public void GameUpdate()
        {
            float forward = _rigidBody.velocity.x;
                
            if (forward != 0 && Math.Abs(forward - _forward) > 0.1f)
            {
                _render.FlipX(forward < 0);
                _forward = forward;
            }
            
            _render.SetSpeed(_rigidBody.velocity.magnitude);
        }

        public void Subscribe()
        {
            _view.Model.Health.SubscribeToValue(_takeDamage);
            _machineSpawner.Spawned += _subscribeToMachine;
        }

        public void Unsubscribe()
        {
            _view.Model.Health.SubscribeToValue(_takeDamage);
            
            if (_currentMachine != null)
            {
                _currentMachine.Connected -= _machineConnect;
            }
        }

        private void _subscribeToMachine(MachineView obj)
        {
            if (_currentMachine != null)
            {
                _currentMachine.Connected -= _machineConnect;
            }

            _currentMachine = obj;
            _currentMachine.Connected += _machineConnect;
        }

        private void _takeDamage(int obj)
        {
            
        }

        private void _machineConnect()
        {
            
        }
    }
}