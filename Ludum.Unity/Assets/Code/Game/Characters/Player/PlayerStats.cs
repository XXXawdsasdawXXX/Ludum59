using System;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    [Serializable]
    public class PlayerStats
    {
        [field: SerializeField] public float SpeedMultiplayer { get; private set; } = 1;
    }
}