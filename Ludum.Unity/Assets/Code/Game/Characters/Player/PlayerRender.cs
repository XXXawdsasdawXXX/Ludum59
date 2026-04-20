using UnityEngine;
using FMODUnity;

namespace Code.Game.Characters.Player
{
    public class PlayerRender : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private SpriteRenderer _renderer;
        
        [Header("FMOD Footsteps")]
        [SerializeField] private EventReference _footstepEvent;

        private float _lastFootstepTime;
        [SerializeField] private float _footstepCooldown = 0.1f;

        public void SetSpeed(float speed)
        {
            Animator.SetFloat(AnimationsHash.Speed, speed);
        }

        public void FlipX(bool flipX)
        {
            _renderer.flipX = flipX;
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