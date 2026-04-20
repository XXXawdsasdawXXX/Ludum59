using System;
using Code.Game.Characters.Player.Abilities;
using Code.Tools;
using Cysharp.Threading.Tasks;
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
        [field: SerializeField] public ReactiveProperty<bool> Connecting { get; private set; } = new(false);
        [field: SerializeField] public int MaxEnergy { get; private set; }
        [field: SerializeField] public RadarModel Radar { get; private set; } = new();
        [field: SerializeField] public StanModel Stan { get; private set; } = new();
        [field: SerializeField] public PathModel Path { get; private set; } = new();

        public Timer AbilityFreeze { get; private set; } = new();

        public PlayerModel(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
        }

        public async void UseAbility()
        {
            SpeedMultiplayer = 0;
            AbilityFreeze.Start(1f);
            await UniTask.Delay(1);
            SpeedMultiplayer = 1;
        }
        
        public void Reset()
        {
            SpeedMultiplayer = 1;
            MaxHealth = _playerConfiguration.MaxHealth;
            Health.SetValueWithoutNotify(MaxHealth);

            MaxEnergy = _playerConfiguration.MaxEnergy;
            Energy.PropertyValue = MaxEnergy;

            Radar = _playerConfiguration.Radar.Clone();
            Radar.ResetPerks();

            Stan = _playerConfiguration.Stan.Clone();
            Stan.ResetPerks();
            
            Path = _playerConfiguration.Path.Clone();
            Path.ResetPerks();
            
            Connecting.PropertyValue = false;
        }
    }
}