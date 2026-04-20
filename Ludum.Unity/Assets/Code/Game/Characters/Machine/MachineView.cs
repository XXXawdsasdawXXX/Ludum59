using System;
using Code.Core.GameLoop;
using Code.Game.Characters;
using Code.Game.Characters.Player;
using UnityEngine;

namespace Code.Game.World
{
    public class MachineView : Character
    {
        public event Action Connected; 
        public event Action EndConnect;

        [field: SerializeField] public Transform ConnectPoint { get; private set; }
        [field: SerializeField] public MachineRender Render { get; private set; }
        
        public override void InitializeComponents()
        {
            
        }

        public void Connect()
        {
            Render.PlayConnect();
        }
        
    }
}