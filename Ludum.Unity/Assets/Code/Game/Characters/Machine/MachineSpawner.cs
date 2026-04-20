using System;
using Code.Game.Characters;
using UnityEngine;

namespace Code.Game.World
{
    public class MachineSpawner : CharacterSpawner<MachineView>
    {
        public event Action<MachineView> Spawned; 
        
        public MachineView Machine;
        
        public void SpawnMachine(Vector3 position)
        {
            Machine = spawn(position);
            Spawned?.Invoke(Machine);
        }
    }
}