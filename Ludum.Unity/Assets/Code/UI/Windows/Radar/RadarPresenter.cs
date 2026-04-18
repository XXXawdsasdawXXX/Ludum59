using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
using Code.Game.Characters.Player;
using Code.Tools;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows
{
    public class RadarPresenter : UIPresenter<RadarView>, IInitializeListener, ISubscriber
    {
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
            _playerSpawner.Player.GetCharacterComponent<PlayerRadar>().Used += OnUsed;
        }

        public void Unsubscribe()
        {
            _playerSpawner.Player.GetCharacterComponent<PlayerRadar>().Used -= OnUsed;
        }

        private void OnUsed()
        {
            Show().Forget();
        }
        
        public async UniTaskVoid Show()
        {
            view.MainCircle.AnimateCircle(20).Forget();
        }
    }
}