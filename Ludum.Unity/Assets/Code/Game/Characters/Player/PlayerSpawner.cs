using System;
using Code.Core.GameLoop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerSpawner : CharacterSpawner<PlayerView>, IInitializeListener, IStartListener
    {
        public event Action<PlayerView> PlayerSpawned;
        public PlayerView Player;

        public UniTask GameInitialize()
        {
            //Player = Spawn(Vector2.zero);

            return UniTask.CompletedTask;
        }

        public UniTask GameStart()
        {
            Player = Spawn(Vector2.zero);

            PlayerSpawned?.Invoke(Player);
            return UniTask.CompletedTask;
        }
    }
}