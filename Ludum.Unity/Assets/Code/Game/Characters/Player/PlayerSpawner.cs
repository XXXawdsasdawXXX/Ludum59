using Code.Core.GameLoop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerSpawner : CharacterSpawner<PlayerView>, IStartListener
    {
        public UniTask GameStart()
        {
            Spawn(Vector2.zero);
            
            return UniTask.CompletedTask;
        }
    }
}