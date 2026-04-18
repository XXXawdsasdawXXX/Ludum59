using System;
using Code.Core.Pools;
using Code.Core.ServiceLocator;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters
{
    public abstract class CharacterSpawner<TCharacter> : MonoBehaviour, IService 
        where TCharacter: Character
    {
        [field: SerializeField] public MonoPool<TCharacter> Pool { get; private set; }
     
        [SerializeField] protected int maxCharactersCount = 1;
        

        [Button]
        public void Spawn(Vector2 position)
        {
            Character character = Pool.GetNext();

            character.InitializeComponents();
            
            character.transform.position = position;
            
            character.Enable();
        }
        
        public bool CanSpawn()
        {
            return Pool.Count() < maxCharactersCount;
        }
        
        //todo despawn


    }
}