using TriInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    public class Chunk : MonoBehaviour
    {
        public static Vector2 CHUNK_SIZE = new(2.5f, 2.5f);
        [field: SerializeField] public Vector3[] EnemiesSpawnPoints { get; private set; }
        [field: SerializeField] public Vector3 PlayerSpawnPoint { get; private set; }
        [field: SerializeField] public Vector3 MachineSpawnPoint { get; private set; }
       
        [SerializeField] private Obstacle[] _obstacles;


        public bool TryGetHeroSpawnPoint(out Vector3 spawnPoint)
        {
            spawnPoint = EnemiesSpawnPoints.Length == 0 ? PlayerSpawnPoint : Vector3.zero;

            spawnPoint += transform.position;

            return EnemiesSpawnPoints.Length == 0;
        }
        
        public bool TryGetMachineSpawnPoint(out Vector3 spawnPoint)
        {
            spawnPoint = EnemiesSpawnPoints.Length == 0 ? MachineSpawnPoint : Vector3.zero;

            spawnPoint += transform.position;

            return EnemiesSpawnPoints.Length == 0;
        }

        public bool TryGetEnemySpawnPoint(out Vector3 spawnPoint)
        {
            spawnPoint = EnemiesSpawnPoints.Length == 0
                ? Vector3.zero
                : EnemiesSpawnPoints[Random.Range(0, EnemiesSpawnPoints.Length)] + transform.position;

            return EnemiesSpawnPoints.Length > 0;
        }

#if UNITY_EDITOR


        [Button]
        private void _findObstacles()
        {
            _obstacles = GetComponentsInChildren<Obstacle>(true);

            EditorUtility.SetDirty(this);
        }

        [Button]
        private void _randomizeSprites()
        {
            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.SetRandomSprite();
            }

            EditorUtility.SetDirty(this);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0.5f, 0.3f, 0.05f);
            Gizmos.DrawCube(transform.position, CHUNK_SIZE);

            if (EnemiesSpawnPoints.Length == 0)
            {
                Gizmos.DrawSphere(transform.position + PlayerSpawnPoint, 0.1f);
                Gizmos.DrawSphere(transform.position + MachineSpawnPoint, 0.15f);
                return;
            }
            
            foreach (Vector3 spawnPoint in EnemiesSpawnPoints)
            {
                Gizmos.DrawSphere(transform.position + spawnPoint, 0.05f);
            }
        }

#endif
        public void RandomizeView()
        {
            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.SetRandomSprite();
            }
        }
    }
}