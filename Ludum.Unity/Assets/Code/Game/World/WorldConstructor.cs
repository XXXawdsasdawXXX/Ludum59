using System.Collections.Generic;
using System.Linq;
using Code.Core.GameLoop;
using Code.Game.Characters.Enemy;
using Code.Game.Characters.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.World
{
    public class WorldConstructor : MonoBehaviour, IStartListener
    {
        [SerializeField] private ChunkMapCreator _chunkMapCreator;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private MachineSpawner _machineSpawner;

        public UniTask GameStart()
        {
            if (_chunkMapCreator.MapChunks.Count == 0)
                _chunkMapCreator.GenerateMap();

            SpawnHeroAndMachine();
            SpawnEnemies();

            return UniTask.CompletedTask;
        }

        public void SpawnMachineAwayFromPlayer()
        {
            if (_playerSpawner.Player == null) return;

            Vector3 playerPos = _playerSpawner.Player.transform.position;
            Vector3 spawnPoint = FindFarthestHeroSpawnPoint(playerPos);
            
            _machineSpawner.SpawnMachine(spawnPoint);
        }

        // ── Private ───────────────────────────────────────────────────────────

        private void SpawnHeroAndMachine()
        {
            List<Chunk> heroChunks = _chunkMapCreator.MapChunks
                .Where(c => c.TryGetHeroSpawnPoint(out _))
                .OrderBy(_ => Random.value) // перемешиваем
                .ToList();

            if (heroChunks.Count == 0) return;

            if (heroChunks.Count == 1)
            {
                heroChunks[0].TryGetHeroSpawnPoint(out Vector3 single);
                _playerSpawner.SpawnHero(single);
                return;
            }

            (Chunk playerChunk, Chunk machineChunk) = FindFarthestPair(heroChunks);

            playerChunk.TryGetHeroSpawnPoint(out Vector3 playerSpawn);
            machineChunk.TryGetHeroSpawnPoint(out Vector3 machineSpawn);

            _playerSpawner.SpawnHero(playerSpawn);
            _machineSpawner.SpawnMachine(machineSpawn);
        }

        private void SpawnEnemies()
        {
            List<Chunk> shuffled = _chunkMapCreator.MapChunks
                .OrderBy(_ => Random.value) // перемешиваем
                .ToList();

            foreach (Chunk chunk in shuffled)
            {
                if (!_enemySpawner.CanSpawn()) break;

                if (chunk.TryGetEnemySpawnPoint(out Vector3 enemySpawn))
                    _enemySpawner.Spawn((EEnemyType)Random.Range(0, 2), enemySpawn);
            }
        }
        private Vector3 FindFarthestHeroSpawnPoint(Vector3 from)
        {
            Chunk best = _chunkMapCreator.MapChunks
                .Where(c => c.TryGetHeroSpawnPoint(out _))
                .OrderByDescending(c => Vector3.Distance(c.transform.position, from))
                .FirstOrDefault();

            if (best == null) return from;

            best.TryGetHeroSpawnPoint(out Vector3 spawnPoint);
            return spawnPoint;
        }

        private static (Chunk, Chunk) FindFarthestPair(List<Chunk> chunks)
        {
            Chunk a = chunks[0];
            Chunk b = chunks[1];
            float maxDist = 0f;

            for (int i = 0; i < chunks.Count; i++)
            {
                for (int j = i + 1; j < chunks.Count; j++)
                {
                    float dist = Vector3.Distance(
                        chunks[i].transform.position,
                        chunks[j].transform.position);

                    if (dist <= maxDist) continue;

                    maxDist = dist;
                    a = chunks[i];
                    b = chunks[j];
                }
            }

            return (a, b);
        }
    }
}