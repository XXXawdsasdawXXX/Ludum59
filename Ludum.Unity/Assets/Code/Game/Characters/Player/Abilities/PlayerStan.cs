using System;
using System.Linq;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
using Code.Tools;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.Characters.Player.Abilities
{
    public class PlayerStan : ICharacterComponent, ISubscriber
    {
        public event Action Used;

        public Condition Condition { get; } = new();

        private readonly EnemySpawner _enemySpawner;
        private readonly PlayerInput _input;
        private readonly PlayerView _view;

        public Timer Cooldown { get; private set; }

        public PlayerStan(PlayerView view)
        {
            _view = view;

            Cooldown = new Timer();

            _enemySpawner = Container.Instance.GetService<EnemySpawner>();
            _input = Container.Instance.GetService<PlayerInput>();
        }

        public void Subscribe()
        {
            _input.StanPressed += _stanPressed;
        }

        public void Unsubscribe()
        {
            _input.StanPressed -= _stanPressed;
        }

        private void _stanPressed()
        {
            Debug.Log(1);
            if (!Condition.AreMet())
            {
                return;
            }

            Debug.Log(2);

            Used?.Invoke();

            _view.Model.Energy.PropertyValue -=
                _view.Model.Stan.EnergyPrice + _view.Model.Stan.PerkEnergyPrice.PropertyValue;

            _updateStan().Forget();
        }

        private async UniTaskVoid _updateStan()
        {
            Cooldown.Start(_view.Model.Stan.Cooldown);

            float radius = _view.Model.Stan.Radius + _view.Model.Stan.PerkRadius.PropertyValue;

            EnemyView[] nearEnemies = _enemySpawner.GetNearEnemies(_view.transform, radius * 0.5f).ToArray();

            Debug.Log($"enemies count = {nearEnemies.Length}. radius = {radius}");
            foreach (EnemyView enemy in nearEnemies)
            {
                enemy.Model.Stan.PropertyValue = true;
            }

            float stanDuration = _view.Model.Stan.Duration + _view.Model.Stan.PerkDuration.PropertyValue;
            await UniTask.Delay(TimeSpan.FromSeconds(stanDuration));

            float speedMultiplier = _view.Model.Stan.PerkEnemySpeedMultiplier.PropertyValue;
            float slowEffectDuration = _view.Model.Stan.SlowEffectDuration.PropertyValue;

            foreach (EnemyView enemy in nearEnemies)
            {
                enemy.Model.SpeedMultiplier.PropertyValue = slowEffectDuration == 0 ? 1 : speedMultiplier;
                enemy.Model.Stan.PropertyValue = false;
            }

            if (slowEffectDuration > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(slowEffectDuration));

                foreach (EnemyView enemy in nearEnemies)
                {
                    enemy.Model.SpeedMultiplier.PropertyValue = 1;
                    enemy.Model.Stan.PropertyValue = false;
                }
            }
        }
    }
}