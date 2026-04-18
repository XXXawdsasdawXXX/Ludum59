using UnityEngine;

namespace Code.Game.Characters.Player
{
    [CreateAssetMenu(fileName = "PlayerConfiguration", menuName = "Configuration/Player")]
    public class PlayerConfiguration : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; } = 1;
    }
}