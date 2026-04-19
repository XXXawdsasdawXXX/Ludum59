using System.Collections.Generic;
using Code.Core.GameLoop;
using Code.Game.World;
using Cysharp.Threading.Tasks;
using PolyNav;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemySpawner : CharacterSpawner<EnemyView>, IStartListener
    {
        [SerializeField] private PolyNavMap _map;
        
        public UniTask GameStart()
        {
            for (int i = 0; i < Random.Range(20, 50); i++)
            {
                if (_map.GetRandomPoint(out Vector2 randomPoint))
                {
                    _spawn((EEnemyType)Random.Range(0,2), randomPoint);
                }

                if (!canSpawn())
                {
                    break;
                }
            }
            
            return UniTask.CompletedTask;
        }

        public IEnumerable<EnemyView> GetNearEnemies(Transform target, float distance)
        {
            IReadOnlyList<EnemyView> allEnabled = Pool.GetAllEnabled();
    
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

        [Button]
        private void _spawn(EEnemyType enemyType, Vector2 position)
        {
            EnemyView character = Pool.GetNext();

            character.transform.position = position;
            
            character.SetType(enemyType);

            character.InitializeComponents();
            
            Spawner.ConnectToGameLoop(character.gameObject);
            
            character.Enable();
        }
    }
}