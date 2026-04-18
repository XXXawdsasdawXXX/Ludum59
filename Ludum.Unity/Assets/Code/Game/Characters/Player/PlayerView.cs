using Code.Core.ServiceLocator;
using UnityEngine;

namespace Code.Game.Characters.Player
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