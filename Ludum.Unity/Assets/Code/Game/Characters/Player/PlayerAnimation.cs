using System;
using Code.Core.GameLoop;
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


        private MachineView _currentMachine;
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");

        private bool _isDied;

        public PlayerAnimation(PlayerView view)
        {
            _rigidBody = view.Rigidbody2D;
            _render = view.Renderer;
            _view = view;
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
        }

        public void Unsubscribe()
        {
            _view.Model.Health.SubscribeToValue(_takeDamage);
        }

        private void _takeDamage(int health)
        {
            if (_isDied)
            {
                return;
            }

            if (health <= 0)
            {
                _isDied = true;
            }

            _render.Animator.SetTrigger(health <= 0 ? Died : TakeDamage);
        }
    }
}