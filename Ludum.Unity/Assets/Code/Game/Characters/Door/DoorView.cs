using Code.Tools;
using UnityEngine;

namespace Code.Game.Characters.Door
{
    public class DoorView : Character
    {
        [field: SerializeField] public ReactiveProperty<bool> IsConnected { get; private set; } = new(false);
        [field: SerializeField] public Transform ConnectPoint { get; private set; }
        [field: SerializeField] public Transform InteractionIcon { get; private set; }
        [field: SerializeField] public Trigger InteractionTrigger { get; private set; }

        public override void InitializeComponents()
        {
        }
    }
}