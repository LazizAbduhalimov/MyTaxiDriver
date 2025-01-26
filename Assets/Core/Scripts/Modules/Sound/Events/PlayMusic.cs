using UnityEngine;
using UnityEngine.Audio;

namespace LSound
{
    public struct PlayMusic
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        
        public void Invoke(AudioClip clip, AudioMixerGroup mixerGroup)
        {
            Clip = clip;
            MixerGroup = mixerGroup;
        }
    }
}