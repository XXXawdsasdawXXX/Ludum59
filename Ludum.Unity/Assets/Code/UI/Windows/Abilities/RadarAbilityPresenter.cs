using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.Game.Characters.Player.Abilities;
using Code.UI.Base;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows
{
    public class RadarAbilityPresenter : UIPresenter<RadarAbilityView>, IInitializeListener, ISubscriber
    {
        private PlayerSpawner _playerSpawner;
        private PlayerRadar _playerRadar;
        
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
            _playerRadar.Cooldown.Updated -= _updateCooldown;
        }

        private void _onPlayerSpawned(PlayerView player)
        {
            if (_playerRadar != null)
            {
                _playerRadar.Cooldown.Updated -= _updateCooldown;
            }
            
            _playerRadar = _playerSpawner.Player.GetCharacterComponent<PlayerRadar>();
            _playerRadar.Cooldown.Updated += _updateCooldown;
        }

        private void _updateCooldown()
        {
            view.Fill.fillAmount = _playerRadar.Cooldown.Current / _playerRadar.Cooldown.Max;
        }
    }
}