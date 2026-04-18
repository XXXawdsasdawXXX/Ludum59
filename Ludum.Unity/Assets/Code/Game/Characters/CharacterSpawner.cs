using Code.Core.Pools;
using Code.Game.Characters;
using TriInspector;
using UnityEngine;

namespace Code.Game
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private MonoPool<Character> _pool;
        [SerializeField] private int _maxCharactersCount = 1;


        [Button]
        public void Spawn(Vector2 position)
        {
            Character character = _pool.GetNext();

            character.InitializeComponents();
            
            character.transform.position = position;
            
            character.Enable();
        }
        
        public bool CanSpawn()
        {
            return _pool.Count() < _maxCharactersCount;
        }
        
        //todo despawn
    }
}