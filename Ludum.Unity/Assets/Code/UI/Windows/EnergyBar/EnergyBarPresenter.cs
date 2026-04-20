using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.UI.Base;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows.HealthBar
{
    public class EnergyBarPresenter : UIPresenter<EnergyBarView>, IInitializeListener ,ISubscriber
    {
        private PlayerSpawner _playerSpawner;

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
            _playerSpawner.Player.Model.Energy.UnsubscibeFromValue(_updateView);
        }

        private void _onPlayerSpawned(PlayerView player)
        {
            player.Model.Energy.SubscribeToValue(_updateView);
            _updateView(_playerSpawner.Player.Model.Health.PropertyValue);
        }
        
        private void _updateView(int value)
        {
            PlayerModel model = _playerSpawner.Player.Model;
            view.Energy.fillAmount = (float)value / model.MaxEnergy;
        }
    }
}