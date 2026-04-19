using System;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    [Serializable]
    public class PlayerModel
    {
        private readonly PlayerConfiguration _playerConfiguration;
        
        [field: SerializeField] public float SpeedMultiplayer { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        public ReactiveProperty<int> Health { get; private set; } = new(0);
        public ReactiveProperty<int> Energy { get; private set; } = new(0);
        public RadarModel Radar { get; private set; } = new RadarModel();
        
        
        public PlayerModel(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
        }

        public void Reset()
        {
            SpeedMultiplayer = 1;
            MaxHealth = _playerConfiguration.MaxHealth;
            Health.PropertyValue = MaxHealth;
            Energy.PropertyValue = _playerConfiguration.MaxEnergy;

            Radar = _playerConfiguration.Radar.Clone();
            Radar.ResetPerks();
        }
    }
}