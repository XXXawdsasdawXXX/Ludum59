using System;
using TriInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    [RequireComponent(typeof(BoxCollider2D))]
    internal class Obstacle : MonoBehaviour
    {
        internal enum EObstacleType
        {
            Vertical,
            Horizontal
        }
        
        internal enum EObstacleSize
        {
            Small,
            Middle,
            Large
        }
        
        
        [ReadOnly, ShowInInspector] public EObstacleType Type { get; private set; }
        [ReadOnly, ShowInInspector] public EObstacleSize Size { get; private set; }

        [SerializeField] private bool _isAnimated;
        [SerializeField] private SpriteRenderer _renderer;
        
        [ShowIf(nameof(_isAnimated))] 
        [SerializeField] private Animator _animation;

        [ShowIf(nameof(_isAnimated))] 
        [SerializeField] private float _animationMaxRandom;
        
        [HideIf(nameof(_isAnimated))]
        [SerializeField] private Sprite[] _spriteVariants;
        
        [SerializeField] private BoxCollider2D _collider;
        private static readonly int Random1 = Animator.StringToHash("Random");

        private void OnEnable()
        {
            if (_isAnimated && _animation != null)
            {
               _animation.SetFloat(Random1, Random.Range(0, _animationMaxRandom - 0.01f));
            }
        }

        [Button]
        public void SetRandomSprite()
        {
            if (_isAnimated || _spriteVariants.Length == 0)
            {
                return;
            }
            
            _renderer.sprite = _spriteVariants[Random.Range(0, _spriteVariants.Length)];
        }
        

#if UNITY_EDITOR

#if UNITY_EDITOR
        // OnValidate вызывается Unity автоматически — никакого [Button]
        private void OnValidate()
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider2D>();

            Vector2 size = _collider.size;
            Type = size.x < size.y ? EObstacleType.Vertical : EObstacleType.Horizontal;

            float scale = Type is EObstacleType.Horizontal ? size.x : size.y;

            Size = scale <= 0.4f ? EObstacleSize.Small
                : scale >= 1f   ? EObstacleSize.Large
                :                 EObstacleSize.Middle;

            // SetDirty нельзя вызывать внутри OnValidate — это вызов из цикла сериализации
            // EditorUtility.SetDirty(this); ← убираем отсюда
        }

        // Отдельная кнопка если нужно принудительно пересчитать вручную
        [Button("Пересчитать тип и размер")]
        private void RecalculateType()
        {
            OnValidate();
            EditorUtility.SetDirty(this);
        }
#endif
#endif
    }
}