using System;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player.Abilities;
using Code.Game.FogOfWar;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerView : Character
    {
        [field: SerializeField] public PlayerRender Renderer { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; set; }
        [field: SerializeField] public PlayerModel Model { get; private set; }
        [field: SerializeField] public FogOfWarUnit FogOfWarUnit { get; private set; } 
        [field: SerializeField] public Trigger RadarTrigger { get; private set; } 
        [field: SerializeField] public CircleCollider2D RadarCircle { get; private set; } 
        
        
        public override void InitializeComponents()
        {
            PlayerConfiguration playerConfiguration = Container.Instance.GetConfiguration<PlayerConfiguration>();

            Model = new PlayerModel(playerConfiguration);
            Model.Reset();

            PlayerMovement movement = new(this);
            Components.Add(typeof(PlayerMovement), movement);

            PlayerCamera playerCamera = new(this);
            Components.Add(typeof(PlayerCamera), playerCamera);

            PlayerFog fog = new(this);
            Components.Add(typeof(PlayerFog), fog);

            PlayerRadar radar = new(this);
            Components.Add(typeof(PlayerRadar), radar);

            PlayerAnimation playerAnimation = new(this);
            Components.Add(typeof(PlayerAnimation), playerAnimation);
            
            radar.Condition.Add(() => Model.Energy.PropertyValue >= Model.Radar.EnergyPrice);
            movement.Condition.Add(() => Model.Health.PropertyValue > 0);
        }

        private void OnDrawGizmos()
        {
            if (Model != null)
            {
                Color color = Color.white;
                color.a = 0.5f;
                Gizmos.color = color;
                
                Gizmos.DrawSphere(transform.position, Model.Radar.Radius);
                
                Gizmos.DrawSphere(transform.position, Model.Radar.Radius + Model.Radar.PerkRadius.PropertyValue);
            }
        }
    }
}