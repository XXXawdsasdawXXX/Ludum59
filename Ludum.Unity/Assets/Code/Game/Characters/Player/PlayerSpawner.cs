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
        
        
        public UniTask GameStart()
        {
            Player = spawn(Vector2.zero);

            PlayerSpawned?.Invoke(Player);
            
            return UniTask.CompletedTask;
        }
    }
}