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
        private readonly PlayerModel _playerModel;
        private readonly PlayerConfiguration _playerConfiguration;
        
        
        public PlayerMovement(PlayerView player)
        {
            _rigidbody2D = player.Rigidbody2D;
            _playerModel = player.Model;

            _playerConfiguration = Container.Instance.GetConfiguration<PlayerConfiguration>();
            _input = Container.Instance.GetService<PlayerInput>();
        }

        public void GameFixedUpdate()
        {
            if (Condition.AreMet())
            {
                Vector2 forward = _input.Forward;
                float speed = _playerConfiguration.Speed * _playerModel.SpeedMultiplayer;
                _rigidbody2D.velocity = forward * speed;
            }
        }
    }
}