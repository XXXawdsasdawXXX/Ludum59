using System;
using Code.Core.GameLoop;
using Code.Core.ServiceLocator;
using UnityEngine;

namespace Code.Game.Characters
{
    public abstract class CharacterSpawner<TCharacter> : MonoBehaviour, IService 
        where TCharacter: Character
    {
        [field: SerializeField] public MonoPool<TCharacter> Pool { get; private set; }
     
        [SerializeField] protected int maxCharactersCount = 1;
        

        public TCharacter Spawn(Vector2 position)
        {
            TCharacter character = Pool.GetNext();

            character.InitializeComponents();
            
            character.transform.position = position;
            
            character.Enable();

            return character;
        }
        
        public bool CanSpawn()
        {
            return Pool.Count() < maxCharactersCount;
        }
        
        //todo despawn


    }
}