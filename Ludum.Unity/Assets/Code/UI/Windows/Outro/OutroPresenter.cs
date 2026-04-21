using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.UI.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Windows.Outro
{
    public class OutroPresenter : UIPresenter<OutroView>, IInitializeListener, ISubscriber
    {
        private PlayerSpawner _playerSpawner;
        private PlayerInput _playerInput;

        private bool _shown;

        public UniTask GameInitialize()
        {
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();
            _playerInput = Container.Instance.GetService<PlayerInput>();
            
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            _playerSpawner.PlayerSpawned += _subscribeToPlayer;
        }

        public void Unsubscribe()
        {
            _playerSpawner.PlayerSpawned -= _subscribeToPlayer;
        }

        private void _subscribeToPlayer(PlayerView obj)
        {
            obj.GetCharacterComponent<PlayerExit>().Exited += _showWindow;
        }

        private async void _showWindow()
        {
            view.Rect.gameObject.SetActive(true);
        }
    }
}