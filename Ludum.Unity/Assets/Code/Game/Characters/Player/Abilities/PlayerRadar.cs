using System;
using System.Collections.Generic;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
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

        private readonly PlayerInput _input;
        private readonly PlayerView _view;

        private readonly AudioConfiguration _audioConfiguration;
        private readonly List<EnemyView> _observedEnemy = new();

        private bool _isActive;


        public PlayerRadar(PlayerView view)
        {
            _view = view;
 
            _input = Container.Instance.GetService<PlayerInput>();
            _audioConfiguration = Container.Instance.GetConfiguration<AudioConfiguration>();

            Cooldown = new Timer();
        }

        public void Subscribe()
        {
            _input.RadarPressed += _onRadarPressed;
            _view.Model.Radar.PerkRadius.SubscribeToValue(_updateRadius);
            _view.RadarTrigger.Enter += _onTriggerEnter;
            _view.RadarTrigger.Exit += _onTriggerExit;
        }
        
        public void Unsubscribe()
        {
            _input.RadarPressed -= _onRadarPressed;
            _view.Model.Radar.PerkRadius.UnsubscibeFromValue(_updateRadius);
            _view.RadarTrigger.Enter -= _onTriggerEnter;
            _view.RadarTrigger.Exit -= _onTriggerExit;
        }

        public void GameUpdate()
        {
            if (_isActive && Cooldown.IsFinish())
            {
                foreach (EnemyView enemyView in _observedEnemy)
                {
                    enemyView.Model.ShowMarker.PropertyValue = false;
                }
                
                _isActive = false;
            }
        }

        private void _onRadarPressed()
        {
            if (Condition.AreMet())
            {
                _isActive = true;

                _view.Model.Energy.PropertyValue -= _view.Model.Radar.EnergyPrice;

                Cooldown.Start(_view.Model.Radar.Duration + _view.Model.Radar.PerkDuration.PropertyValue);

                RuntimeManager.PlayOneShot(_audioConfiguration.Radar);

                foreach (EnemyView enemyView in _observedEnemy)
                {
                    enemyView.Model.ShowMarker.PropertyValue = true;
                }
                
                Used?.Invoke();
            }
        }
        
        private void _updateRadius(float perkRadius)
        {
            _view.RadarCircle.radius = _view.Model.Radar.Radius + perkRadius;
        }
        
        private void _onTriggerExit(GameObject obj)
        {
            if (obj.TryGetComponent(out EnemyView enemyView))
            {
                enemyView.Model.ShowMarker.PropertyValue = false;
                _observedEnemy.Remove(enemyView);
            }
        }

        private void _onTriggerEnter(GameObject obj)
        {
            if (obj.TryGetComponent(out EnemyView enemyView))
            {
                if (_isActive)
                {
                    enemyView.Model.ShowMarker.PropertyValue = true;
                }
                
                _observedEnemy.Add(enemyView);
            }
        }
    }
}