using System;
using Code.Game.Characters;
using Code.Game.Characters.Player;
using Code.Tools;
using UnityEngine;

namespace Code.Game.World
{
    public class MachineView : Character
    {
        private PlayerInput _playerInput;

        [field: SerializeField] public ReactiveProperty<bool> IsConnected { get; private set; } = new(false);
        [field: SerializeField] public Transform ConnectPoint { get; private set; }
        [field: SerializeField] public MachineRender Render { get; private set; }
        [field: SerializeField] public Transform InteractionIcon { get; private set; }
        [field: SerializeField] public Trigger InteractionTrigger { get; private set; }



        public override void InitializeComponents()
        {
            
        }

     
    }
}