using UnityEngine;

namespace Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "GlobalAudioSettings", menuName = "Scriptable Objects/GlobalAudioSettings")]
    public class GlobalAudioSettings : ScriptableObject
    {
        public bool Mute;
        
        [Range(0f,1f)]
        public float Volume = 1f;
        
        
    }
}