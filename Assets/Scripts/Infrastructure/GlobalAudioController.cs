using Infrastructure.Scriptables;
using UnityEngine;

namespace Infrastructure
{
    public class GlobalAudioController : MonoBehaviour
    {
        public GlobalAudioSettings AudioSettings;

        private void FixedUpdate()
        {
            AudioListener.pause = AudioSettings.Mute;
            AudioListener.volume = AudioSettings.Volume;
        }
    }
}