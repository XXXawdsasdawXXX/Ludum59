using System;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerSpawner : CharacterSpawner<PlayerView>
    {
        public event Action<PlayerView> PlayerSpawned;
        public PlayerView Player { get; private set; }
        
        public void SpawnHero(Vector2 position)
        {
            Player = spawn(position);

            PlayerSpawned?.Invoke(Player);
        }
    }
}