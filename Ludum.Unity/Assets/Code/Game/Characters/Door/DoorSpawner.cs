using System;
using UnityEngine;

namespace Code.Game.Characters.Door
{
    public class DoorSpawner : CharacterSpawner<DoorView>
    {
        public event Action<DoorView> Spawned;

        public DoorView Door;
        
        public void Spawn(Vector3 position)
        {
            Door = spawn(position);
            
            Spawned?.Invoke(Door);
        }
    }
}