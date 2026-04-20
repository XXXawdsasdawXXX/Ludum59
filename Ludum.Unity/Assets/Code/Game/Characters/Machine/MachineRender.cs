using UnityEngine;

namespace Code.Game.World
{
    public class MachineRender : MonoBehaviour
    {
        private static readonly int Connect = Animator.StringToHash("Connect");
        [field: SerializeField] public Animator Animator { get; private set; }


        public void PlayConnect()
        {
            Animator.SetTrigger(Connect);
        }
    }
}