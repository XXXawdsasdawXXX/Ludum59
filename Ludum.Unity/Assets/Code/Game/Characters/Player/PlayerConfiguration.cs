using UnityEngine;

namespace Code.Game.Characters.Player
{
    [CreateAssetMenu(fileName = "PlayerConfiguration", menuName = "Configuration/Player")]
    public class PlayerConfiguration : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; } = 1;
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public int MaxEnergy { get; private set; } = 100;

        [field: Header("Radar")]
        [field: SerializeField] public int RadarEnergyPrice { get; private set; } = 1;
        [field: SerializeField] public int RadarCooldown { get; private set; } = 8;
        [field: SerializeField] public int RadarPerkCooldown { get; private set; } = -3;
        
        [field: Header("Stan")]
        [field: SerializeField] public int StanEnergyPrice { get; private set; } = 1;
        
    }
}