using UnityEngine;
using UnityEngine.Audio;

namespace LSound
{
    public struct PlaySound
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public Vector3 Position;
        public Transform Parent;

        public void Invoke(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position, Transform parent = null)
        {
            Clip = clip;
            MixerGroup = mixerGroup;
            Position = position;
            Parent = parent;
        }
    }
}