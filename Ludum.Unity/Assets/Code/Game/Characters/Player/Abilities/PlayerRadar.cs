using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Audio;
using Code.Tools;
using FMODUnity;

namespace Code.Game.Characters.Player
{
    public class PlayerRadar : ICharacterComponent, ISubscriber
    {
        public Action Used;
        public Condition Condition { get; } = new();
        
        private readonly PlayerModel _model;
        private readonly PlayerInput _input;
        private readonly PlayerConfiguration _configuration;
        private readonly AudioConfiguration _audioConfiguration;

        public Timer Cooldown { get; private set; }

        public PlayerRadar(PlayerView view)
        {
            _model = view.Model;
            _input = Container.Instance.GetService<PlayerInput>();
            
            _configuration = Container.Instance.GetConfiguration<PlayerConfiguration>();
            _audioConfiguration = Container.Instance.GetConfiguration<AudioConfiguration>();

            Cooldown = new Timer(_configuration.RadarCooldown);
            Cooldown.Finish();
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
            if (Condition.AreMet() && Cooldown.AreMet())
            {
                Used?.Invoke();
                
                _model.Energy.PropertyValue -= _configuration.RadarEnergyPrice;
                
                RuntimeManager.PlayOneShot(_audioConfiguration.Radar);
                
                Cooldown.Reset();
            }
        }
    }
}