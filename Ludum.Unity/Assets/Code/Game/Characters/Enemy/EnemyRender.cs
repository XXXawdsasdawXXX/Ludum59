using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyRender : MonoBehaviour
    {
        [SerializeField] public Animator _animator;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetModel(EnemyModel model)
        {
            _spriteRenderer.sprite = model.Sprite;
            _animator.runtimeAnimatorController = model.Animator;
        }
        
        public void SetSpeed(float speed)
        {
            _animator.SetFloat(AnimationsHash.Speed, speed);
        }

        public void FlipX(bool flipX)
        {
            _spriteRenderer.flipX = flipX;
        }
    }
}