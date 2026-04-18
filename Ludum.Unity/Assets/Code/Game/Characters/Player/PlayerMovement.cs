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
        
        public PlayerMovement(PlayerView player)
        {
            _rigidbody2D = player.Rigidbody2D;
            
            _playerStats = player.Stats;
            
            _playerConfiguration = Container.Instance.GetConfiguration<PlayerConfiguration>();

            _input = Container.Instance.GetService<PlayerInput>();
        }

        public void GameFixedUpdate()
        {
            if (Condition.AreMet())
            {
                float speed = _playerConfiguration.Speed * _playerStats.SpeedMultiplayer;
                _rigidbody2D.velocity = _input.Forward.PropertyValue * speed;
            }
        }
    }
}