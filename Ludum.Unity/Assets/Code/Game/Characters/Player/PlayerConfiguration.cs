using UnityEngine;

namespace Code.Game.Characters.Player
{
    [CreateAssetMenu(fileName = "PlayerConfiguration", menuName = "Configuration/Player")]
    public class PlayerConfiguration : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; } = 1;

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        
        [field: SerializeField] public int MaxEnergy { get; private set; } = 100;
        
        [field: SerializeField] public int RadarEnergyPrice { get; private set; } = 1;
    }
}