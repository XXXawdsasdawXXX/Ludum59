using System;
using Code.Tools;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    [Serializable]
    public class EnemyModel
    {
        [field: SerializeField] public EEnemyType Type { get; private set; }
        [field: SerializeField, PreviewObject] public Sprite Sprite { get; private set; }
        [field: SerializeField] public AnimatorOverrideController Animator { get; private set; }
        [field: SerializeField] public int Size { get; private set; }
        [field: SerializeField] public float Speed { get; private set; } = 1;
        [field: SerializeField] public ReactiveProperty<bool> Follow { get; private set; } = new(false);
        [field: SerializeField] public ReactiveProperty<bool> AbilityAgro { get; private set; } = new(false);

        public ReactiveProperty<float> SpeedMultiplier = new(1);
        public ReactiveProperty<bool> ShowMarker = new(false);
        [field: SerializeField] public ReactiveProperty<bool> Stan = new(false);
        [field: SerializeField] public ReactiveProperty<bool> Attack = new(false);

        
        public EnemyModel Clone()
        {
            return new EnemyModel
            {
                Type = Type,
                Sprite = Sprite,
                Animator = Animator,
                Size = Size,
                Speed = Speed, 
                Follow = new ReactiveProperty<bool>(false),
                SpeedMultiplier = new ReactiveProperty<float>(SpeedMultiplier.PropertyValue),
                ShowMarker = new ReactiveProperty<bool>(false),
                Stan = new ReactiveProperty<bool>(false),
                Attack = new ReactiveProperty<bool>(false),
                AbilityAgro = new ReactiveProperty<bool>(false)
            };
        }
    }
}