using System;
using Code.Core.GameLoop;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerDeath : ICharacterComponent, ISubscriber
    {
        private readonly PlayerView _view;
        public Condition Condition { get; } = new();
        public event Action Died;

        private bool _isDied;
        public PlayerDeath(PlayerView view)
        {
            _view = view;
        }

        public void Subscribe()
        {
            _view.Model.Health.SubscribeToValue(_check);
        }

        public void Unsubscribe()
        {
            _view.Model.Health.UnsubscibeFromValue(_check);
        }

        private void _check(int health)
        {
            if (health <= 0 && (!_isDied || _view.Rigidbody2D.velocity.magnitude > 0))
            {
                _view.Rigidbody2D.velocity = Vector2.zero;
                
                _view.Model.SpeedMultiplayer = 0;
                
                Died?.Invoke();
            }
        }
    }
}