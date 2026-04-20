using System.Collections.Generic;
using System.Linq;
using Code.Core.GameLoop;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Code.Game.World
{
    public class ChunkMapCreator : MonoBehaviour
    {
        private static readonly Vector2 CHUNK_SIZE = new(2.5f, 2.5f);

        [SerializeField] private Vector2Int _mapSize = new(11, 11);
        [SerializeField] private int _maxRegenerateAttempts = 10;
        [SerializeField] private Chunk[] _prefabs;
        [field: SerializeField] public List<Chunk> MapChunks { get; private set; } = new();

        // Генерирует карту и перегенерирует если не хватает точек спавна
        public bool GenerateMapWithRequirements(int requiredHeroPoints, int requiredEnemyPoints)
        {
            for (int attempt = 0; attempt < _maxRegenerateAttempts; attempt++)
            {
                ClearMap();
                GenerateMap();

                int heroCount  = MapChunks.Count(c => c.TryGetHeroSpawnPoint(out _));
                int enemyCount = MapChunks.Count(c => c.TryGetEnemySpawnPoint(out _));

                if (heroCount >= requiredHeroPoints && enemyCount >= requiredEnemyPoints)
                {
                    Debug.Log($"[ChunkMapCreator] Карта сгенерирована за {attempt + 1} попыток. " +
                              $"Hero points: {heroCount}, Enemy points: {enemyCount}");
                    return true;
                }

                Debug.LogWarning($"[ChunkMapCreator] Попытка {attempt + 1}: недостаточно точек " +
                                 $"(hero: {heroCount}/{requiredHeroPoints}, enemy: {enemyCount}/{requiredEnemyPoints}), перегенерация...");
            }

            Debug.LogError($"[ChunkMapCreator] Не удалось сгенерировать карту с нужным количеством точек за {_maxRegenerateAttempts} попыток");
            return false;
        }

        [Button]
        public void GenerateMap()
        {
            if (_prefabs == null || _prefabs.Length == 0)
            {
                Debug.LogWarning("[ChunkCreator] has not chunk prefabs");
                return;
            }

            for (int y = 0; y < _mapSize.y; y++)
            {
                for (int x = 0; x < _mapSize.x; x++)
                {
                    Chunk prefab = _prefabs[Random.Range(0, _prefabs.Length)];

                    Vector3 position = transform.position + new Vector3(
                        x * CHUNK_SIZE.x,
                        y * CHUNK_SIZE.y,
                        0f);

                    prefab.RandomizeView();
#if UNITY_EDITOR
                    Chunk chunk = (Chunk)PrefabUtility.InstantiatePrefab(prefab, transform);
#else
                    Chunk chunk = Spawner.Instantiate(prefab, transform);
#endif
                    chunk.transform.position = position;
                    chunk.name = $"{chunk.name} [{x},{y}]";
                    MapChunks.Add(chunk);
                }
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            Debug.Log($"[ChunkCreator] generated count: {MapChunks.Count}");
#endif
        }

        [Button]
        public void ClearMap()
        {
            foreach (Chunk chunk in MapChunks)
            {
                if (chunk == null) continue;
                DestroyImmediate(chunk.gameObject);
            }

            MapChunks.Clear();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 mapCenter = transform.position + new Vector3(
                _mapSize.x * CHUNK_SIZE.x * 0.5f - CHUNK_SIZE.x * 0.5f,
                _mapSize.y * CHUNK_SIZE.y * 0.5f - CHUNK_SIZE.y * 0.5f,
                0f);

            Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
            Gizmos.DrawCube(mapCenter, new Vector3(_mapSize.x * CHUNK_SIZE.x, _mapSize.y * CHUNK_SIZE.y, 0f));

            Gizmos.color = new Color(1f, 1f, 0f, 0.6f);
            Gizmos.DrawWireCube(mapCenter, new Vector3(_mapSize.x * CHUNK_SIZE.x, _mapSize.y * CHUNK_SIZE.y, 0f));
        }
#endif
    }
}