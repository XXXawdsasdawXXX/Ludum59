using System;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player.Abilities
{
    [Serializable]
    public class StanModel
    {
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public ReactiveProperty<float> PerkRadius { get; private set; } = new(0);

        [field: SerializeField] public float Duration{ get; private set; }
        [field: SerializeField] public ReactiveProperty<float> PerkDuration { get; private set; } = new(0);
        
        [field: SerializeField] public int EnergyPrice{ get; private set; }
        [field: SerializeField] public ReactiveProperty<int> PerkEnergyPrice { get; private set; } = new(0);
        
        [field: SerializeField] public ReactiveProperty<float> PerkEnemySpeedMultiplier { get; private set; } = new(0);
        [field: SerializeField] public ReactiveProperty<float> SlowEffectDuration { get; private set; } = new(0);
        
        
        public StanModel Clone()
        {
            return new StanModel
            {
                Cooldown = Cooldown,
                Radius = Radius,
                PerkRadius = new ReactiveProperty<float>(PerkRadius.PropertyValue),

                Duration = Duration,
                PerkDuration = new ReactiveProperty<float>(PerkDuration.PropertyValue),

                EnergyPrice = EnergyPrice,
                PerkEnergyPrice = new ReactiveProperty<int>(PerkEnergyPrice.PropertyValue),

                PerkEnemySpeedMultiplier = new ReactiveProperty<float>(PerkEnemySpeedMultiplier.PropertyValue),
                SlowEffectDuration = new ReactiveProperty<float>(SlowEffectDuration.PropertyValue)
            };
        }

        public void ResetPerks()
        {
            PerkRadius.PropertyValue = 0;
            PerkDuration.PropertyValue = 0;
            PerkEnergyPrice.PropertyValue = 0;
            PerkEnemySpeedMultiplier.PropertyValue = 1;
            SlowEffectDuration.PropertyValue = 0;
        }
    }
}