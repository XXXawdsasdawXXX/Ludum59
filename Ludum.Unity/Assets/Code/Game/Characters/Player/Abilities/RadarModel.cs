using System;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player.Abilities
{
    [Serializable]
    public class RadarModel
    {
        public int EnergyPrice;
        
        [Space] public float Duration;
        public ReactiveProperty<float> PerkDuration;
        
        [Space] public float Cooldown;
        public ReactiveProperty<float> PerkCooldown;
        
        [Space] public float Radius;
        public ReactiveProperty<float> PerkRadius;
        
        [Space] public ReactiveProperty<bool> ShownCoolStuff;
        
        [Space] public ReactiveProperty<bool> ShowEnemyForwardDuration;

        public RadarModel Clone()
        {
            return new RadarModel
            {
                EnergyPrice = EnergyPrice,
                
                Duration = Duration,
                PerkDuration = new ReactiveProperty<float>(PerkDuration.PropertyValue),

                Cooldown = Cooldown,
                PerkCooldown = new ReactiveProperty<float>(PerkCooldown.PropertyValue),

                Radius = Radius,
                PerkRadius = new ReactiveProperty<float>(PerkRadius.PropertyValue),

                ShownCoolStuff = new ReactiveProperty<bool>(ShownCoolStuff.PropertyValue),
                ShowEnemyForwardDuration = new ReactiveProperty<bool>(ShowEnemyForwardDuration.PropertyValue),
            };
        }

        public void ResetPerks()
        {
            PerkRadius.SetValueWithoutNotify(0);   
            PerkDuration.SetValueWithoutNotify(0);   
            ShownCoolStuff.SetValueWithoutNotify(false);   
            ShowEnemyForwardDuration.SetValueWithoutNotify(false);   
        }
    }
}