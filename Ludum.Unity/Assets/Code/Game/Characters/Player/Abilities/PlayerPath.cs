using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Audio;
using Code.Game.Characters.Enemy;
using Code.Game.World;
using Code.Tools;
using Cysharp.Threading.Tasks;
using FMODUnity;

namespace Code.Game.Characters.Player.Abilities
{
    public class PlayerPath : ICharacterComponent, ISubscriber
    {
        public Condition Condition { get; } = new Condition();
        public Timer Cooldown { get; } = new Timer();
        
        private readonly PlayerView _view;
        private readonly PlayerInput _input;
        private readonly MachineSpawner _machineSpawner;
        private readonly PixelTrail _pixelTrail;
        private readonly EnemySpawner _enemySpawner;
        private readonly SoundConfiguration _soundConfiguration;

        public PlayerPath(PlayerView view)
        {
            _view = view;
            _input = Container.Instance.GetService<PlayerInput>();
            _machineSpawner = Container.Instance.GetService<MachineSpawner>();
            _pixelTrail = Container.Instance.GetService<PixelTrail>();
            _enemySpawner = Container.Instance.GetService<EnemySpawner>();
            _soundConfiguration = Container.Instance.GetConfiguration<SoundConfiguration>();
        }
        
        public void Subscribe()
        {
            _input.PathPressed += _activate;
        }

        public void Unsubscribe()
        {
            _input.PathPressed -= _activate;
        }

        private void _activate()
        {
            if (!Condition.AreMet())
            {
                return;
            }
            
            RuntimeManager.PlayOneShot(_soundConfiguration.Path);
            
            _view.UseAbility();
            
            float cooldown = _view.Model.Path.Cooldown + _view.Model.Path.PerkCooldown.PropertyValue;
            
            Cooldown.Start(cooldown);
            
            float duration = _view.Model.Path.Duration + _view.Model.Path.PerkDuration.PropertyValue;
          
            _pixelTrail.Activate(duration, _view.transform.position, _machineSpawner.Machine.transform.position);
            
            _agro(duration);
        }

        private async void _agro(float duration)
        {
            float radius = _view.Model.Path.AgroRadius + _view.Model.Path.AgroPerkRadius.PropertyValue;

            EnemyView[] enemies = _enemySpawner.GetNearEnemies(_view.transform, radius).ToArray();

            foreach (EnemyView enemyView in enemies)
            {
                enemyView.Model.AbilityAgro.PropertyValue = true;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            
            foreach (EnemyView enemyView in enemies)
            {
                enemyView.Model.AbilityAgro.PropertyValue = false;
            }
        }
    }
}