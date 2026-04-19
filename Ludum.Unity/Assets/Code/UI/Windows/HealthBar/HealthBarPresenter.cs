using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.UI.Base;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows.HealthBar
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
            UpdateView(_playerSpawner.Player.Model.Health.PropertyValue);
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _playerSpawner.Player.Model.Health.SubscribeToValue(UpdateView);
        }

        public void Unsubscribe()
        {
            _playerSpawner.Player.Model.Health.UnsubscibeFromValue(UpdateView);
        }

        private void UpdateView(int value)
        {
            PlayerModel model = _playerSpawner.Player.Model;
            view.Health.fillAmount = (float)value / model.MaxHealth;
        }
    }
}