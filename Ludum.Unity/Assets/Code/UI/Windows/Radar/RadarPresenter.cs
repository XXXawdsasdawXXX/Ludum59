using System.Collections.Generic;
using System.Linq;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
using Code.Game.Characters.Player;
using Code.Game.Characters.Player.Abilities;
using Code.UI.Base;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Code.UI.Windows.Radar
{
    public class RadarPresenter : UIPresenter<RadarView>, IInitializeListener, ISubscriber
    {
        [ShowInInspector, ReadOnly] private bool _isActive;
        
        private EnemySpawner _enemySpawner;
        private PlayerSpawner _playerSpawner;
        private RadarModel _radarModel;
        
        public UniTask GameInitialize()
        {
            _enemySpawner = Container.Instance.GetService<EnemySpawner>();
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
            _playerSpawner.Player.GetCharacterComponent<PlayerRadar>().Used -= _onUsed;
        }

        private void _onPlayerSpawned(PlayerView player)
        {
            player.GetCharacterComponent<PlayerRadar>().Used += _onUsed;
            _radarModel = player.Model.Radar;
        }

        private void _onUsed()
        {
            if (_isActive)
            {
                Debug.LogWarning("Radar is active right now");
                
                return;
            }
            
            _show().Forget();
        }

        private async UniTaskVoid _show()
        {
            _isActive = true;
            
            view.Rect.gameObject.SetActive(true);
            
            float duration = _radarModel.Duration + _radarModel.PerkDuration.PropertyValue;
        
            view.MainCircle.AnimateSize(duration * 0.7f);
            view.MainCircle.AnimateAlpha(duration * 0.5f);

            float radius = _radarModel.Radius + _radarModel.PerkRadius.PropertyValue;
            
            await UniTask.WaitUntil(() => view.MainCircle.AnimationsCount.PropertyValue == 0);

            _isActive = false;
            
            view.Rect.gameObject.SetActive(false);
        }
    }
}