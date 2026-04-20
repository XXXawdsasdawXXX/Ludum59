using System;
using Code.Core.GameLoop;
using Code.Game.Characters;
using UnityEngine;

namespace Code.Game.World
{
    public class MachineView : Character, IPoolEntity
    {
        public event Action Connected; 
        public override void InitializeComponents()
        {
            
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}