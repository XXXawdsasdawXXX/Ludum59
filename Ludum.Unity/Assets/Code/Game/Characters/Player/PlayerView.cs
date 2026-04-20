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

            PlayerStan stan = new(this);
            Components.Add(typeof(PlayerStan), stan);
            
            PlayerPath path = new(this);
            Components.Add(typeof(PlayerPath), path);
            
            PlayerConnection connection = new(this);
            Components.Add(typeof(PlayerConnection), connection);
            
            stan.Condition.Add(() => stan.Cooldown.IsFinish());
            stan.Condition.Add(() => Model.Energy.PropertyValue >= 
                                     Model.Stan.EnergyPrice + Model.Stan.PerkEnergyPrice.PropertyValue);
            stan.Condition.Add(() => Model.Health.PropertyValue > 0);
            stan.Condition.Add(() => !Model.Connecting.PropertyValue);
            stan.Condition.Add(() => Model.AbilityFreeze.IsFinish());
            
            radar.Condition.Add(() => radar.Cooldown.IsFinish());
            radar.Condition.Add(() => Model.Energy.PropertyValue >= Model.Radar.EnergyPrice);
            radar.Condition.Add(() => Model.Health.PropertyValue > 0);
            radar.Condition.Add(() => !Model.Connecting.PropertyValue);
            radar.Condition.Add(() => Model.AbilityFreeze.IsFinish());
            
            path.Condition.Add(() => path.Cooldown.IsFinish());
            path.Condition.Add(() => Model.Energy.PropertyValue >= Model.Path.EnergyPrice);
            path.Condition.Add(() => Model.Health.PropertyValue > 0);
            path.Condition.Add(() => !Model.Connecting.PropertyValue);
            path.Condition.Add(() => Model.AbilityFreeze.IsFinish());
            
            movement.Condition.Add(() => Model.Health.PropertyValue > 0);
            movement.Condition.Add(() => !Model.Connecting.PropertyValue);
            movement.Condition.Add(() => Model.AbilityFreeze.IsFinish());
            
            connection.Condition.Add(() => Model.Health.PropertyValue > 0);
            connection.Condition.Add(() => Model.AbilityFreeze.IsFinish());
        }

        public void UseAbility()
        {
            Model.UseAbility();
            Renderer.HandActivate();
            Rigidbody2D.velocity = Vector2.zero;
        }
    }
}