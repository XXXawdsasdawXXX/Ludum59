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

        [SerializeField] private int _requiredHeroPoints  = 6;
        [SerializeField] private int _requiredEnemyPoints = 10;

        // Чанки которые уже заняты — не будут использованы повторно
        private readonly HashSet<Chunk> _occupiedChunks = new();

        public UniTask GameStart()
        {
            _occupiedChunks.Clear();

            _chunkMapCreator.GenerateMapWithRequirements(_requiredHeroPoints, _requiredEnemyPoints);

            SpawnHeroAndFirstMachine();
            SpawnEnemies();

            return UniTask.CompletedTask;
        }

        public void SpawnMachineAwayFromPlayer()
        {
            if (_playerSpawner.Player == null) return;

            Vector3 playerPos = _playerSpawner.Player.transform.position;

            Chunk best = _chunkMapCreator.MapChunks
                .Where(c => !_occupiedChunks.Contains(c) && c.TryGetHeroSpawnPoint(out _))
                .OrderByDescending(c => Vector3.Distance(c.transform.position, playerPos))
                .FirstOrDefault();

            if (best == null)
            {
                Debug.LogWarning("[WorldConstructor] Нет свободных точек для машины");
                return;
            }

            best.TryGetHeroSpawnPoint(out Vector3 spawnPoint);
            _occupiedChunks.Add(best);
            SpawnMachineAndSubscribe(spawnPoint);
        }

        // Возвращает N рассеянных точек — только из свободных чанков
        public List<Vector3> GetSpreadSpawnPoints(int count)
        {
            List<Chunk> candidates = _chunkMapCreator.MapChunks
                .Where(c => !_occupiedChunks.Contains(c) && c.TryGetHeroSpawnPoint(out _))
                .OrderBy(_ => Random.value)
                .ToList();

            if (candidates.Count == 0) return new List<Vector3>();

            var selected = new List<Chunk> { candidates[Random.Range(0, candidates.Count)] };

            while (selected.Count < count && selected.Count < candidates.Count)
            {
                Chunk best = candidates
                    .Except(selected)
                    .OrderByDescending(c => selected.Min(s =>
                        Vector3.Distance(c.transform.position, s.transform.position)))
                    .First();

                selected.Add(best);
            }

            var result = new List<Vector3>();
            foreach (Chunk chunk in selected)
            {
                chunk.TryGetHeroSpawnPoint(out Vector3 point);
                _occupiedChunks.Add(chunk); // помечаем как занятый
                result.Add(point);
            }

            return result;
        }

        // ── Private ───────────────────────────────────────────────────────────

        private void SpawnHeroAndFirstMachine()
        {
            List<Vector3> points = GetSpreadSpawnPoints(2);

            if (points.Count == 0) return;

            _playerSpawner.SpawnHero(points[0]);

            if (points.Count >= 2)
                SpawnMachineAndSubscribe(points[1]);
        }

        private void SpawnMachineAndSubscribe(Vector3 position)
        {
            _machineSpawner.SpawnMachine(position);
            _machineSpawner.Machine.IsConnected.SubscribeToValue(_onMachineConnected);
        }

        private void _onMachineConnected(bool isConnected)
        {
            if (!isConnected) return;

            _machineSpawner.Machine.IsConnected.UnsubscibeFromValue(_onMachineConnected);

            if (!_machineSpawner.CanSpawn()) return;

            SpawnMachineAwayFromPlayer();
        }

        private void SpawnEnemies()
        {
            List<Chunk> shuffled = _chunkMapCreator.MapChunks
                .OrderBy(_ => Random.value)
                .ToList();

            foreach (Chunk chunk in shuffled)
            {
                if (!_enemySpawner.CanSpawn()) break;

                if (chunk.TryGetEnemySpawnPoint(out Vector3 enemySpawn))
                    _enemySpawner.Spawn((EEnemyType)Random.Range(0, 2), enemySpawn);
            }
        }
    }
}