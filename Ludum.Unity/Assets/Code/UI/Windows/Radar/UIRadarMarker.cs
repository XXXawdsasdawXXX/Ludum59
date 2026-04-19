using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using Code.Game.Characters.Enemy;
using Code.Game.Characters.Player;
using Code.Tools;
using Code.UI.Base;
using Code.UI.MPUIKit.Runtime.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.Radar
{
    public class UIRadarMarker : UIComponent, IInitializeListener, ISubscriber
    {
        private const Ease ANIMATION_EASE = Ease.OutQuad;
        
        private static readonly Color _color = new(0.25f, 0.6f, 0.35f, 1f);
        [field: SerializeField] public MPImage Image { get; private set; }
        [field: SerializeField] public Image SimpleImage { get; private set; }
        
        [SerializeField, ReadOnly] private Vector2 _defaultSize;
        
        [SerializeField, CanBeNull] private EnemyView _enemyView;
        public ReactiveProperty<int> AnimationsCount { get; } = new(0);
        
        private Tween _sizeTween;
        private Tween _alphaTween;
        private PlayerSpawner _playerSpawner;


        public UniTask GameInitialize()
        {
            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();
            return UniTask.CompletedTask;
        }

        public void Subscribe()
        {
            if (_enemyView != null)
            {
                 _enemyView.Model.ShowMarker.SubscribeToValue(_updateEnemyMarker);
            }
        }

        public void Unsubscribe()
        {
            if (_enemyView != null)
            {
                _enemyView.Model.ShowMarker.UnsubscibeFromValue(_updateEnemyMarker);
            }
        }

        public void AnimateSize(float duration)
        {
            AnimationsCount.PropertyValue++;
            
            Rect.sizeDelta = Vector2.zero;
            
            _sizeTween?.Complete();
            _sizeTween = Rect.DOSizeDelta(_defaultSize, duration)
                .SetEase(ANIMATION_EASE)
                .OnComplete(() =>
                {
                    AnimationsCount.PropertyValue--;
                });
        }

        public void AnimateAlpha(float duration)
        {
            AnimationsCount.PropertyValue++;

            Image.color = _color;
            
            _alphaTween?.Complete();
            _alphaTween = Image.DOColor(Color.clear, duration)
                .SetEase(ANIMATION_EASE)
                .OnComplete(() =>
                {
                    AnimationsCount.PropertyValue--;
                });
        }

        private void _updateEnemyMarker(bool show)
        {
            if (show)
            {
                SimpleImage.color = Color.white;
                Color targetColor = SimpleImage.color;
                targetColor.a = 0.3f;
                Rect.gameObject.SetActive(true);
                _alphaTween = SimpleImage.DOColor(targetColor, 0.7f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(gameObject, LinkBehaviour.KillOnDisable);
            }
            else
            {
                _alphaTween?.Kill();
                Rect.gameObject.SetActive(false);
            }
        }

        public override void Disable()
        {
            base.Disable();

            AnimationsCount.PropertyValue = 0;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (Rect == null)
            {
                return;
            }

            _defaultSize = Rect.sizeDelta;
        }
#endif

    
    }
}