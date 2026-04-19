using System.Collections.Generic;
using System.Linq;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
using Code.Game.Characters.Player;
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
        
        public UniTask GameInitialize()
        {
            _enemySpawner = Container.Instance.GetService<EnemySpawner>();
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();

            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _playerSpawner.PlayerSpawned += PlayerSpawnerOnPlayerSpawned;
    
        }

        private void PlayerSpawnerOnPlayerSpawned(PlayerView obj)
        {
            obj.GetCharacterComponent<PlayerRadar>().Used += OnUsed;
        }

        public void Unsubscribe()
        {
            _playerSpawner.PlayerSpawned -= PlayerSpawnerOnPlayerSpawned;
            _playerSpawner.Player.GetCharacterComponent<PlayerRadar>().Used -= OnUsed;
        }

        private void OnUsed()
        {
            if (_isActive)
            {
                Debug.LogWarning("Radar is active right now");
                return;
            }
            
            Show().Forget();
        }

        private async UniTaskVoid Show()
        {
            _isActive = true;

            view.MainCircle.AnimateSize(4);
            view.MainCircle.AnimateAlpha(5);

            IReadOnlyList<EnemyView> enemies = _enemySpawner.Pool.GetAllEnabled();
            
            Vector3 playerPos = _playerSpawner.Player.transform.position;

            List<EnemyView> sorted = enemies
                .OrderBy(e => Vector3.Distance(e.transform.position, playerPos))
                .ToList();

            foreach (EnemyView enemyView in sorted)
            {
                UIRadarMarker marker = view.MarkerPool.GetNext();
                
                marker.AnimationsCount.SubscribeToValue(value =>
                {
                    if (value == 0)
                    {
                        marker.AnimationsCount.ClearSubscription();
                        view.MarkerPool.Disable(marker);
                    }
                });
                
                marker.Follow(enemyView.transform);
                marker.AnimateAlpha(8);
                marker.AnimateSize(7);

                await UniTask.WaitForSeconds(Random.Range(0, 1));
            }

            await UniTask.WaitUntil(() => view.MainCircle.AnimationsCount.PropertyValue == 0);

            _isActive = false;
        }
    }
}