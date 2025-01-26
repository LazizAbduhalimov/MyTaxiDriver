using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class AllSounds : MonoBehaviour
    {
        [Space(5f)] 
        public AudioClip[] Musics;
        
        public AudioMixerGroup MusicMixerGroup;
        public AudioMixerGroup SfxMixerGroup;
    }
}