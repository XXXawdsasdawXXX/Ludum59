using System;
using Code.Core.GameLoop;
using Code.Game.Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    public class MachineSpawner : CharacterSpawner<MachineView>, IStartListener
    {
        public MachineView MachineView;
        
        [SerializeField] private Vector2[] _spawnPoints;
        
        public UniTask GameStart()
        {
          MachineView= spawn(_spawnPoints[Random.Range(0, _spawnPoints.Length)]);
            
            return UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Vector2 spawnPoint in _spawnPoints)
            {
                Gizmos.DrawSphere(spawnPoint, 0.3f );
            }
        }
#endif
    }
}