using System.Collections.Generic;
using Code.Core.GameLoop;
using PolyNav;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemySpawner : CharacterSpawner<EnemyView>
    {
        [SerializeField] private PolyNavMap _map;
        
        
        [Button]
        public void Spawn(EEnemyType enemyType, Vector2 position)
        {
            EnemyView character = Pool.GetNext();

            character.transform.position = position;

            character.SetType(enemyType);

            character.InitializeComponents();
            
            Spawner.ConnectToGameLoop(character.gameObject);
            
            character.Enable();
        }
        
        public IEnumerable<EnemyView> GetNearEnemies(Transform target, float distance)
        {
            IReadOnlyList<EnemyView> allEnabled = Pool.GetAllEnabled();
    
            Vector3 center = target.position;
            float r = distance;

// горизонтальная линия
            Debug.DrawLine(
                center + Vector3.left * r,
                center + Vector3.right * r,
                Color.yellow,
                2f
            );

// вертикальная линия
            Debug.DrawLine(
                center + Vector3.up * r,
                center + Vector3.down * r,
                Color.yellow,
                2f);
            
            float sqrDistance = distance * distance;
            Vector3 targetPos = target.position;
    
            List<EnemyView> result = new(allEnabled.Count);
    
            foreach (EnemyView t in allEnabled)
            {
                Vector3 diff = t.transform.position - targetPos;
                float sqrMag = diff.x * diff.x + diff.y * diff.y;

                if (sqrMag <= sqrDistance)
                {
                    result.Add(t);
                }
            }
            
            return result;
        }

    }
}