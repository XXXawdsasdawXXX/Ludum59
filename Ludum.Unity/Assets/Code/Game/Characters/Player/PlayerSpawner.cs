using Code.Core.GameLoop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerSpawner : CharacterSpawner<PlayerView>, IInitializeListener
    {
        public PlayerView Player;

        public UniTask GameInitialize()
        {
            Player = Spawn(Vector2.zero);

            return UniTask.CompletedTask;
        }
    }
}