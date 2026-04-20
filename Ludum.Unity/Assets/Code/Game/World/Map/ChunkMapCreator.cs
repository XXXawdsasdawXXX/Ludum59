using System.Collections.Generic;
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
        [SerializeField] private Chunk[] _prefabs;
        [field: SerializeField] public List<Chunk> MapChunks { get; private set; } = new();


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
        private void ClearMap()
        {
            foreach (Chunk chunk in MapChunks)
            {
                if (chunk == null)
                {
                    continue;
                }
                
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
            // Показываем границы всей карты
            Vector3 mapCenter = transform.position + new Vector3(
                _mapSize.x * CHUNK_SIZE.x * 0.5f - CHUNK_SIZE.x * 0.5f,
                _mapSize.y * CHUNK_SIZE.y * 0.5f - CHUNK_SIZE.y * 0.5f,
                0f);

            Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
            Gizmos.DrawCube(mapCenter, new Vector3(
                _mapSize.x * CHUNK_SIZE.x,
                _mapSize.y * CHUNK_SIZE.y,
                0f));

            Gizmos.color = new Color(1f, 1f, 0f, 0.6f);
            Gizmos.DrawWireCube(mapCenter, new Vector3(
                _mapSize.x * CHUNK_SIZE.x,
                _mapSize.y * CHUNK_SIZE.y,
                0f));
        }

#endif
    }
}