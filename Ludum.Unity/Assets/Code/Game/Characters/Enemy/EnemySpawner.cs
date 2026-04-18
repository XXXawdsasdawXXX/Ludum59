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
                Spawn((EEnemyType)Random.Range(0,2), position);
            }
            
            return UniTask.CompletedTask;
        }

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