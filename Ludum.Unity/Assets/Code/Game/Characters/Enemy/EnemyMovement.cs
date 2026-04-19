using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.Game.World;
using Code.Tools;
using Cysharp.Threading.Tasks;
using PolyNav;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyMovement : ICharacterComponent, IUpdateListener
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

        
        public void GameUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _agent.SetDestination(_playerSpawner.Pool.GetAll()[0].transform.position);
            }
        }
    }
}