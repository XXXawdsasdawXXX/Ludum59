using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows
{
    public class HealthBarPresenter : UIPresenter<HealthBarView>, IInitializeListener, ISubscriber
    {
        private PlayerSpawner _playerSpawner;

        public UniTask GameInitialize()
        {
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();

            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _playerSpawner.Player.Stats.Health.SubscribeToValue(UpdateView);
        }

        public void Unsubscribe()
        {
            _playerSpawner.Player.Stats.Health.UnsubscibeFromValue(UpdateView);
        }

        private void UpdateView(int value)
        {
            var stats = _playerSpawner.Player.Stats;
            view.Health.fillAmount = (float)stats.Health.PropertyValue / stats.MaxHealth;
        }
    }
}