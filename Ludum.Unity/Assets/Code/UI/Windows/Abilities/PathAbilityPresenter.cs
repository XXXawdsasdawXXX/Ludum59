using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.Game.Characters.Player.Abilities;
using Code.UI.Base;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Code.UI.Windows
{
    public class PathAbilityPresenter  : UIPresenter<AbilityView>, IInitializeListener, ISubscriber
    {
        private PlayerSpawner _playerSpawner;
        private PlayerPath _path;
        
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
            if (_path != null)
            {
                _path.Used -= Used;
            }
            
            _path = _playerSpawner.Player.GetCharacterComponent<PlayerPath>();
            _path.Used += Used;
        }

        private void Used()
        {
            view.Fill.fillAmount = 0;

            float abilityDuration = _playerSpawner.Player.Model.Path.Duration 
                                    + _playerSpawner.Player.Model.Path.PerkDuration.PropertyValue;
            
            Tween tween = view.Background
                .DOFade(0, 0.5f)
                .SetLoops(-1, LoopType.Yoyo);
            
            DOVirtual.DelayedCall(abilityDuration, () => tween.Kill());

            view.Fill.DOFillAmount(1, _path.Cooldown.Duration);
        }
    }
}