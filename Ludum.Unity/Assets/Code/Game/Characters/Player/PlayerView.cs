using FoW;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerView : Character
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; set; }
        [field: SerializeField] public PlayerStats Stats { get; set; }
        
        [field: SerializeField] public FogOfWarTeam FogOfWarTeam { get; private set; } 
      
        [field: SerializeField] public FogOfWarUnit FogOfWarUnit { get; private set; } 
        
        
        public override void InitializeComponents()
        {
            Stats = new PlayerStats();
            
            PlayerMovement movement = new(this);
            Components.Add(typeof(PlayerMovement), movement);

            PlayerCamera playerCamera = new(this);
            Components.Add(typeof(PlayerCamera), playerCamera);

            PlayerFog fog = new PlayerFog(this);
            Components.Add(typeof(PlayerFog), fog);
            
            movement.Condition.Add(() => true);
        }
    }
}