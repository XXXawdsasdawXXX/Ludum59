using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows
{
    public class HealthBarPresenter : UIPresenter<HealthBarView>, IInitializeListener,IStartListener ,ISubscriber
    {
        private PlayerSpawner _playerSpawner;

        public UniTask GameInitialize()
        {
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();

            return UniTask.CompletedTask;
        }

        public UniTask GameStart()
        {
            UpdateView(_playerSpawner.Player.Stats.Health.PropertyValue);
            
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
            PlayerStats stats = _playerSpawner.Player.Stats;
            view.Health.fillAmount = (float)value / stats.MaxHealth;
        }
    }
}