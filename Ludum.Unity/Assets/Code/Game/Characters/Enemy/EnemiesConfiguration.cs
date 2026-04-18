using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    [CreateAssetMenu(fileName = "EnemiesConfiguration", menuName = "Configuration/Enemies")]
    public class EnemiesConfiguration : ScriptableObject
    {
        [field: SerializeField] public List<EnemyModel> Models { get; private set; }
    }
}