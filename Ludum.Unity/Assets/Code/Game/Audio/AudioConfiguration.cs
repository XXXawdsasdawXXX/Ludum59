using FMODUnity;
using UnityEngine;

namespace Code.Game.Audio
{
    [CreateAssetMenu(fileName = "AudioConfiguration", menuName = "Configuration/Audio")]
    public class AudioConfiguration : ScriptableObject
    {
        [field: SerializeField] public EventReference Radar { get; private set; } 
    }
}