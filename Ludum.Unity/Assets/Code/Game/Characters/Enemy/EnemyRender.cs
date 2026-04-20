using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyRender : MonoBehaviour
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Stan = Animator.StringToHash("Stan");
    
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

        public void SetAttack(bool value)
        {
            _animator.SetBool(Attack, value);
        }
        
        public void SetStan(bool value)
        {
            _animator.SetBool(Stan, value);
        }
    }
}