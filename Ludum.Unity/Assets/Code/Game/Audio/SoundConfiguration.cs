using FMODUnity;
using UnityEngine;

namespace Code.Game.Audio
{
    [CreateAssetMenu(fileName = "AudioConfiguration", menuName = "Configuration/Audio")]
    public class SoundConfiguration : ScriptableObject
    {
        [field: SerializeField] public EventReference Radar { get; private set; } 
        [field: SerializeField] public EventReference Stan { get; private set; }
        [field: SerializeField]   public EventReference Path { get; private set; }
    }
}