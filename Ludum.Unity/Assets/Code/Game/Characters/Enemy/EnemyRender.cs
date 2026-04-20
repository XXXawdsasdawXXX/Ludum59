using FMODUnity;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyRender : MonoBehaviour
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Stan = Animator.StringToHash("Stan");
    
        [SerializeField] public Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        
        [Header("FMOD Footsteps")]
        [SerializeField] private EventReference _footstepEvent;
        [Header("FMOD Attack")]
        [SerializeField] private EventReference _attakEvent;
        
        private float _lastFootstepTime;
        [SerializeField] private float _footstepCooldown = 0.1f;

        
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
        
        public void PlayAttak()
        {
            RuntimeManager.PlayOneShot(_attakEvent, transform.position);
        }
        public void SetStan(bool value)
        {
            _animator.SetBool(Stan, value);
        }

        public void PlayFootstep()
        {
            if (Time.time - _lastFootstepTime < _footstepCooldown)
                return;

            _lastFootstepTime = Time.time;

            RuntimeManager.PlayOneShot(_footstepEvent, transform.position);
        }
    }
}