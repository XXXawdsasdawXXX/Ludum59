using System;
using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Player.Abilities
{
    [Serializable]
    public class PathModel 
    {
        public int EnergyPrice;

        [Space] public float Duration;
        public ReactiveProperty<float> PerkDuration;
        
        [Space] public float Cooldown;
        public ReactiveProperty<float> PerkCooldown;
        
        [Space] public float AgroRadius;
        public ReactiveProperty<float> AgroPerkRadius;
        
        
        public PathModel Clone()
        {
            return new PathModel
            {
                EnergyPrice = EnergyPrice,
                
                Duration = Duration,
                PerkDuration = new ReactiveProperty<float>(PerkDuration.PropertyValue),

                Cooldown = Cooldown,
                PerkCooldown = new ReactiveProperty<float>(PerkCooldown.PropertyValue),

                AgroRadius = AgroRadius,
                AgroPerkRadius = new ReactiveProperty<float>(AgroPerkRadius.PropertyValue),
            };
        }

        public void ResetPerks()
        {
            AgroPerkRadius.SetValueWithoutNotify(0);   
            PerkDuration.SetValueWithoutNotify(0);
        }
    }
}