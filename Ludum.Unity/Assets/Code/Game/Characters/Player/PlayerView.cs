using Code.Game.Characters;
using Code.Game.Characters.Player;
using UnityEngine;

namespace Code.Game.Player
{
    public class PlayerView : Character
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; set; }
        [field: SerializeField] public PlayerStats Stats { get; set; }

        public override void InitializeComponents()
        {
            Stats = new PlayerStats();
            
            PlayerMovement movement = new(this);
            Components.Add(typeof(PlayerMovement), movement);

            
            movement.Condition.Add(() => true);
        }
    }
}