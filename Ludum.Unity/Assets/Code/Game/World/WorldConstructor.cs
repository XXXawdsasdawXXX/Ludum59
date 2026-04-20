using System.Collections.Generic;
using Code.Core.GameLoop;
using Code.Game.Characters.Enemy;
using Code.Game.Characters.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Game.World
{
    public class WorldConstructor : MonoBehaviour, IInitializeListener
    {
        [SerializeField] private ChunkMapCreator _chunkMapCreator;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private MachineSpawner _machineSpawner;

        public UniTask GameInitialize()
        {
            _chunkMapCreator.ClearMap();
            _chunkMapCreator.GenerateMap();
            
            foreach (Chunk mapChunk in _chunkMapCreator.MapChunks)
            {
                if (mapChunk.TryGetHeroSpawnPoint(out Vector3 spawnPoint))
                {
                    if (_playerSpawner.Player == null)
                    {
                        _playerSpawner.SpawnHero(spawnPoint);
                    }
                    else if (_machineSpawner.Machine == null)
                    {
                        _machineSpawner.SpawnMachine(spawnPoint);
                    }
                }
            }

            foreach (Chunk mapChunk in _chunkMapCreator.MapChunks)
            {
                if (mapChunk.TryGetEnemySpawnPoint(out Vector3 enemySpawnPoint))
                {
                    _enemySpawner.Spawn((EEnemyType)Random.Range(0, 2), enemySpawnPoint);
                }
            }

            return UniTask.CompletedTask;
        }
    }
}