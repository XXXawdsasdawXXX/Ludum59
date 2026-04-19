using Code.Core.ServiceLocator;
using Code.Game.FogOfWar;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerView : Character
    {
        [field: SerializeField] public PlayerRender Renderer { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; set; }
        [field: SerializeField] public PlayerStats Stats { get; set; }
        [field: SerializeField] public FogOfWarUnit FogOfWarUnit { get; private set; } 
        
        
        public override void InitializeComponents()
        {
            PlayerConfiguration playerConfiguration = Container.Instance.GetConfiguration<PlayerConfiguration>();
            
            Stats = new PlayerStats
            {
                Energy =
                {
                    PropertyValue = playerConfiguration.MaxEnergy
                },
                Health =
                {
                    PropertyValue = playerConfiguration.MaxHealth
                }
            };

            PlayerMovement movement = new(this);
            Components.Add(typeof(PlayerMovement), movement);

            PlayerCamera playerCamera = new(this);
            Components.Add(typeof(PlayerCamera), playerCamera);

            PlayerFog fog = new(this);
            Components.Add(typeof(PlayerFog), fog);

            PlayerRadar radar = new(this);
            Components.Add(typeof(PlayerRadar), radar);

            PlayerAnimation playerAnimation = new PlayerAnimation(this);
            Components.Add(typeof(PlayerAnimation), playerAnimation);
            
            radar.Condition.Add(() => Stats.Energy.PropertyValue >= playerConfiguration.RadarEnergyPrice);
            movement.Condition.Add(() => Stats.Health.PropertyValue > 0);
        }
    }
}