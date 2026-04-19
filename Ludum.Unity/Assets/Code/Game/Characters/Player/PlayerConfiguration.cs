using Code.Game.Characters.Player.Abilities;
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
        [field: SerializeField] public RadarModel Radar { get; private set; }
        
        [field: Header("Stan")]
        [field: SerializeField] public int StanEnergyPrice { get; private set; } = 1;
        
    }
}