using System.Collections.Generic;
using Code.Core.GameLoop;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Code.Game.Characters.Enemy
{
    public class EnemySpawner : CharacterSpawner<EnemyView>, IStartListener
    {
        public UniTask GameStart()
        {
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                Vector2 position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                _spawn((EEnemyType)Random.Range(0,2), position);
            }
            
            return UniTask.CompletedTask;
        }

        public List<EnemyView> GetNearEnemies(Transform target, float distance)
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

            character.InitializeComponents();

            character.SetType(enemyType);

            character.transform.position = position;

            character.Enable();
        }
    }
}