using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemySpawner : CharacterSpawner<EnemyView>
    {
        [Button]
        public void Spawn(EEnemyType enemyType, Vector2 position)
        {          
            EnemyView character = Pool.GetNext();
            
            character.InitializeComponents();

            character.SetType(enemyType);
            
            character.transform.position = position;
            
            character.Enable();
        }
    }
}