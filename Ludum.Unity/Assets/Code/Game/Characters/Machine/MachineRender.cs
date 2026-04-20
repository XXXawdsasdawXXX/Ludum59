using Code.Game.Characters;
using UnityEngine;

namespace Code.Game.World
{
    public class MachineRender : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }


        public void PlayConnect()
        {
            Animator.SetTrigger(AnimationsHash.Connection);
        }
    }
}