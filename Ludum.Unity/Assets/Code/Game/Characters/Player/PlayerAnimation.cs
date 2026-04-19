using System;
using System.Security.Cryptography;
using Code.Core.GameLoop;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerAnimation : ICharacterComponent, IUpdateListener
    {
        public Condition Condition { get; } = new Condition();
        
        private readonly Rigidbody2D _rigidBody;
        private readonly PlayerRender _render;

        private float _forward;
        public PlayerAnimation(PlayerView view)
        {
            _rigidBody = view.Rigidbody2D;
            _render = view.Renderer;
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
    }
}