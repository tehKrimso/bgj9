using Infrastructure.Scriptables;
using UnityEngine;

namespace Infrastructure
{
    public class GlobalAudioController : MonoBehaviour
    {
        public bool UseGlobalControl;
        public GlobalAudioSettings AudioSettings;

        private void FixedUpdate()
        {
            if(!UseGlobalControl)
                return;
            
            AudioListener.pause = AudioSettings.Mute;
            AudioListener.volume = AudioSettings.Volume;
        }
    }
}