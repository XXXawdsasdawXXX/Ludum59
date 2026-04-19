using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Tools;
using FMODUnity;
using UnityEngine;
using AudioConfiguration = Code.Game.Audio.AudioConfiguration;

namespace Code.Game.Characters.Player.Abilities
{
    public class PlayerRadar : ICharacterComponent, ISubscriber, IUpdateListener
    {
        public Action Used;
        public Condition Condition { get; } = new();
        public Timer Cooldown { get; }
        
        private readonly PlayerModel _model;
        private readonly PlayerInput _input;

        private readonly AudioConfiguration _audioConfiguration;
        

        public PlayerRadar(PlayerView view)
        {
            Debug.Log("construct radar");
            _model = view.Model;
            _input = Container.Instance.GetService<PlayerInput>();

            _audioConfiguration = Container.Instance.GetConfiguration<AudioConfiguration>();

            Cooldown = new Timer(_model.Radar.Cooldown);
            Cooldown.Finish();
        }
        
        public void Subscribe()
        {
            _input.RadarPressed += _onRadarPressed;
            
            _model.Radar.PerkDuration.SubscribeToValue(_updateCooldownValue);
        }

        public void Unsubscribe()
        {
            _input.RadarPressed -= _onRadarPressed;
        }

        public void GameUpdate()
        {
            if (Cooldown.AreMet())
            {
                return;
            }

            Cooldown.Update(Time.deltaTime);
        }

        private void _onRadarPressed()
        {
            if (Condition.AreMet() && Cooldown.AreMet())
            {
                Used?.Invoke();
                
                _model.Energy.PropertyValue -= _model.Radar.EnergyPrice;
                
                RuntimeManager.PlayOneShot(_audioConfiguration.Radar);
                
                Cooldown.Reset();
            }
        }

        private void _updateCooldownValue(float perkCooldown)
        {
            Cooldown.SetMaxTime(_model.Radar.Cooldown + perkCooldown);
        }
    }
}