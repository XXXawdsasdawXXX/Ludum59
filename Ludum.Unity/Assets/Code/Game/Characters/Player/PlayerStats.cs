using System;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    [Serializable]
    public class PlayerStats
    {
        [field: SerializeField] public float SpeedMultiplayer { get; private set; } = 1;
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        public ReactiveProperty<int> Health { get; private set; } = new(0);
        public ReactiveProperty<int> Energy { get; private set; } = new(0);
    }
}