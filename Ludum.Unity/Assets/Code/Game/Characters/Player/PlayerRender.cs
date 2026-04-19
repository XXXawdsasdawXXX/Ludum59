using UnityEngine;

namespace Code.Game.Characters.Player
{
    public class PlayerRender : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _renderer;

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(AnimationsHash.Speed, speed);
        }

        public void FlipX(bool flipX)
        {
            _renderer.flipX = flipX;
        }
    }
}