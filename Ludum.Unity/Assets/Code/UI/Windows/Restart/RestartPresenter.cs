using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Player;
using Code.UI.Base;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI.Windows.Restart
{
    public class RestartPresenter : UIPresenter<RestartView>, IInitializeListener, ISubscriber
    {
        private PlayerSpawner _playerSpawner;
        private PlayerInput _playerInput;

        private bool _shown;
        private static readonly int Play = Animator.StringToHash("Play");

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
            obj.GetCharacterComponent<PlayerDeath>().Died += _showWindow;
        }

        private async void _showWindow()
        {
            view.CanvasGroup.alpha = 0;

            view.Rect.gameObject.SetActive(true);

            view.CanvasGroup.DOFade(1, 3).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
            {
            //    view.TVAnimator.SetTrigger(Play);
            });

            await UniTask.WaitUntil(() => _playerInput.InteractionPressed);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}