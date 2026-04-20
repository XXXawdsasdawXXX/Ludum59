using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.UI.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Windows.Health
{
    public class HealthBarPresenter : UIPresenter<HealthBarView>, IInitializeListener, ISubscriber
    {
        private PlayerSpawner _playerSpawner;
        private PlayerModel _model;
        private static readonly int Glitch = Animator.StringToHash("Glitch");


        public UniTask GameInitialize()
        {
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _playerSpawner.PlayerSpawned += _onPlayerSpawned;
        }

        public void Unsubscribe()
        {
            _playerSpawner.PlayerSpawned -= _onPlayerSpawned;
        }

        private void _onPlayerSpawned(PlayerView player)
        {
            if (_model != null)
            {
                _model.Health.UnsubscibeFromValue(_updateBar);
            }

            _model = player.Model;
            
            _model.Health.SubscribeToValue(_updateBar);
        }

        private void _updateBar(int value)
        {
            if (value < 0)
            {
                return;
            }
            view.Hand.sprite = view.HandStates[Math.Min(value, view.HandStates.Length)];
            view.DisplayAnimator.SetTrigger(Glitch);
        }
    }
}