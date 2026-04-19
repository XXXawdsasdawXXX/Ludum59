using Code.Core.GameLoop;
using Code.Tools;
using Code.UI.Base;
using Code.UI.MPUIKit.Runtime.Scripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace Code.UI.Windows.Radar
{
    public class UIRadarMarker : UIComponent, IInitializeListener, IUpdateListener
    {
        private const Ease ANIMATION_EASE = Ease.OutQuad;
        
        private static readonly Color _color = new(0.25f, 0.6f, 0.35f, 1f);
        
        [field: SerializeField] public MPImage Image { get; private set; }
        
        [SerializeField, ReadOnly] private Vector2 _defaultSize;
        public ReactiveProperty<int> AnimationsCount { get; } = new(0);

        private Camera _camera;
        private Transform _follow;
        private Tween _sizeTween;
        private Tween _alphaTween;

        
        public UniTask GameInitialize()
        {
            _camera = Camera.main; 
            
            return UniTask.CompletedTask;
        }

        public void GameUpdate()
        {
            if (_camera == null || _follow == null || AnimationsCount.PropertyValue == 0)
            {
                return;
            }

            transform.position = _camera.WorldToScreenPoint(_follow.position);
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

        public void Follow(Transform follow)
        {
            _follow = follow;
        }

        public override void Disable()
        {
            base.Disable();

            AnimationsCount.PropertyValue = 0;
            
            _follow = null;
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