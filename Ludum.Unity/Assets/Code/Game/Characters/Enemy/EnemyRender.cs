using FMODUnity;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyRender : MonoBehaviour
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Stan = Animator.StringToHash("Stan");
    
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Header("FMOD Footsteps")]
        [SerializeField] private EventReference _footstepEvent;
        
        [Header("FMOD Attack TV")]
        [SerializeField] private EventReference _attackTvEvent;
        
        [Header("FMOD Attack Default")]
        [SerializeField] private EventReference _attackDefaultEvent;
        
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

        public void SetStan(bool value)
        {
            _animator.SetBool(Stan, value);
        }
        
        private void PlayOneShot(EventReference eventRef)
        {
            if (eventRef.IsNull) return;

            RuntimeManager.PlayOneShot(eventRef, transform.position);
        }
        
        public void PlayAttackTV()
        {
            PlayOneShot(_attackTvEvent);
        }

        public void PlayAttackDefault()
        {
            PlayOneShot(_attackDefaultEvent);
        }

        public void PlayFootstep()
        {
            if (Time.time - _lastFootstepTime < _footstepCooldown)
                return;

            _lastFootstepTime = Time.time;

            PlayOneShot(_footstepEvent);
        }
    }
}