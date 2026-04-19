using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.Game.Characters.Player.Abilities;
using Code.Game.World;
using Code.Tools;
using PolyNav;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyMovement : ICharacterComponent, ISubscriber, IUpdateListener
    {
        public Condition Condition { get; private set; } = new();
        
        private readonly MapView _map;
        
        private readonly PlayerSpawner _playerSpawner;
        private readonly PolyNavAgent _agent;

        public EnemyMovement(EnemyView view)
        {
            _agent = view.Agent;
            
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();
        }

        public void Subscribe()
        {
            _playerSpawner.Player.GetCharacterComponent<PlayerRadar>().Used += OnRadarUsed;
        }

        public void GameUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _agent.SetDestination(_playerSpawner.Pool.GetAll()[0].transform.position);
            }
        }

        public void Unsubscribe()
        {
            _playerSpawner.Player.GetCharacterComponent<PlayerRadar>().Used -= OnRadarUsed;
        }

        private void OnRadarUsed()
        {
            
        }
    }
}