using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerMovement : ICharacterComponent, IFixedUpdateListener
    {
        public Condition Condition { get; } = new();

        private readonly Rigidbody2D _rigidbody2D;
        private readonly PlayerInput _input;
        private readonly PlayerStats _playerStats;
        private readonly PlayerConfiguration _playerConfiguration;
        private readonly SpriteRenderer _renderer;
        
        public PlayerMovement(PlayerView player)
        {
            _rigidbody2D = player.Rigidbody2D;
            _playerStats = player.Stats;
            _renderer = player.Renderer;

            _playerConfiguration = Container.Instance.GetConfiguration<PlayerConfiguration>();
            _input = Container.Instance.GetService<PlayerInput>();
        }

        public void GameFixedUpdate()
        {
            if (Condition.AreMet())
            {
                Vector2 forward = _input.Forward.PropertyValue;
                
                if (forward.x != 0 && Mathf.RoundToInt(forward.x) != Mathf.RoundToInt(_rigidbody2D.velocity.x))
                {
                    _renderer.flipX = forward.x > 0;
                }
                
                float speed = _playerConfiguration.Speed * _playerStats.SpeedMultiplayer;
                _rigidbody2D.velocity = forward * speed;
            }
        }
    }
}