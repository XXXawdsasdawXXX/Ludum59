using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.Game.World;
using Code.Tools;
using PolyNav;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyMovement : ICharacterComponent, ISubscriber, IUpdateListener
    {
        public Condition Condition { get; } = new();
        
        private readonly PolyNavAgent _agent;
        private readonly EnemyView _view;
        private readonly Vector3 _startPoint;
        private readonly Transform _playerTransform;


        public EnemyMovement(EnemyView view, Vector3 startPoint)
        {
            _view = view;
            _startPoint = startPoint;
            _agent = view.Agent;

            PlayerSpawner playerSpawner = Container.Instance.GetService<PlayerSpawner>();
            MapView map = Container.Instance.GetView<MapView>();

            _playerTransform = playerSpawner.Player.transform;
            
            _agent.maxSpeed = view.Model.Speed;
            _agent.maxForce = view.Model.Speed;
            _agent.map = map.Maps[Math.Min(map.Maps.Length - 1, view.Model.Size)];
        }

        public void Subscribe()
        {
            _view.Model.SpeedMultiplier.SubscribeToValue(_updateSpeed);
            _view.Model.Follow.SubscribeToValue(_moveToStartPoint);
        }

        public void Unsubscribe()
        {
            _view.Model.SpeedMultiplier.UnsubscibeFromValue(_updateSpeed);
            _view.Model.Follow.UnsubscibeFromValue(_moveToStartPoint);
        }

        private bool _isStopped = false;

        public void GameUpdate()
        {
            if (Condition.AreMet())
            {
                if (_isStopped)
                {
                    _isStopped = false;
                }
                _agent.SetDestination(_playerTransform.position);
            }
            else
            {
                if (!_isStopped)
                {
                    _agent.Stop();
                    _isStopped = true;
                }
            }
        }

        private void _moveToStartPoint(bool obj)
        {
            if (obj)
            {
                return;
            }
            
            _agent.SetDestination(_startPoint);
        }

        private void _updateSpeed(float multiplier)
        {
            _agent.maxSpeed = _view.Model.Speed * multiplier;
        }
    }
}