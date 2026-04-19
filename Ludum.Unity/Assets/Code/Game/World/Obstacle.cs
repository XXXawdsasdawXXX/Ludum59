using System;
using TriInspector;
using UnityEditor;
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

        [SerializeField] private SpriteRenderer _renderer;

        [SerializeField] private Sprite[] _spriteVariants;
        
        [SerializeField] private BoxCollider2D _collider;

        
        [Button]
        public void SetRandomSprite()
        {
            if (_spriteVariants.Length == 0)
            {
                return;
            }
            
            _renderer.sprite = _spriteVariants[Random.Range(0, _spriteVariants.Length)];

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(_renderer);
#endif
        }
        

#if UNITY_EDITOR

        [Button]
        private void OnValidate()
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider2D>();
            }

            Vector2 size = _collider.size;
            Type = size.x < size.y ? EObstacleType.Vertical : EObstacleType.Horizontal;

            float scale = Type is EObstacleType.Horizontal ? size.x : size.y;

            if (scale <= 0.4)
            {
                Size = EObstacleSize.Small;
            }
            else if(scale >= 1)
            {
                Size = EObstacleSize.Large;
            }
            else
            {
                Size = EObstacleSize.Middle;
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}