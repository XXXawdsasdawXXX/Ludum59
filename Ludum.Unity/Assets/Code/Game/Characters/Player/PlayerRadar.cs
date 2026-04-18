using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Tools;

namespace Code.Game.Characters.Player
{
    public class PlayerRadar : ICharacterComponent, ISubscriber
    {
        public Action Used;
        
        public Condition Condition { get; private set; } = new Condition();
        
        private readonly PlayerStats _stats;
        private readonly PlayerInput _input;
        private readonly PlayerConfiguration _configuration;


        public PlayerRadar(PlayerView view)
        {
            _stats = view.Stats;
            _input = Container.Instance.GetService<PlayerInput>();
            _configuration = Container.Instance.GetConfiguration<PlayerConfiguration>();
        }
        
        public void Subscribe()
        {
            _input.RadarPressed += RadarPressed;
        }

        public void Unsubscribe()
        {
            _input.RadarPressed -= RadarPressed;
        }

        private void RadarPressed()
        {
            if (Condition.AreMet())
            {
                Used?.Invoke();
                _stats.Energy.PropertyValue -= _configuration.RadarEnergyPrice;
            }
        }
    }
}