using System;
using Code.Core.GameLoop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerSpawner : CharacterSpawner<PlayerView>, IStartListener
    {
        public event Action<PlayerView> PlayerSpawned;
        public PlayerView Player { get; private set; }

        [SerializeField] private Vector2 _spawnPosition;
        
        public UniTask GameStart()
        {
            Player = spawn(_spawnPosition);

            PlayerSpawned?.Invoke(Player);
            
            return UniTask.CompletedTask;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_spawnPosition, 0.1f);
        }
    }
}