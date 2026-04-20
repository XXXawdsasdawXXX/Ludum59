using System;
using Code.Game.Characters.Player.Abilities;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player
{
    [Serializable]
    public class PlayerModel
    {
        private readonly PlayerConfiguration _playerConfiguration;
        
        [field: SerializeField] public float SpeedMultiplayer { get; set; }
      
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public ReactiveProperty<int> Health { get; private set; } = new(0);
        
        [field: SerializeField] public ReactiveProperty<int> Energy { get; private set; } = new(0);
        [field: SerializeField] public int MaxEnergy { get; private set; }
        [field: SerializeField] public RadarModel Radar { get; private set; } = new();
        [field: SerializeField] public StanModel Stan { get; private set; } = new();
        [field: SerializeField] public PathModel Path { get; private set; } = new();


        public PlayerModel(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
        }

        public void Reset()
        {
            SpeedMultiplayer = 1;
            MaxHealth = _playerConfiguration.MaxHealth;
            Health.SetValueWithoutNotify(MaxHealth);

            MaxEnergy = _playerConfiguration.MaxEnergy;
            Energy.SetValueWithoutNotify(MaxEnergy);

            Radar = _playerConfiguration.Radar.Clone();
            Radar.ResetPerks();

            Stan = _playerConfiguration.Stan.Clone();
            Stan.ResetPerks();
            
            Path = _playerConfiguration.Path.Clone();
            Path.ResetPerks();
        }
    }
}