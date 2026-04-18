using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemyRender : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetModel(EnemyModel model)
        {
            _spriteRenderer.sprite = model.Sprite;
        }
    }
}