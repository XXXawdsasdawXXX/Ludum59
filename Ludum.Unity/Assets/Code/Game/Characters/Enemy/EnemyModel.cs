using System;
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
    }
}