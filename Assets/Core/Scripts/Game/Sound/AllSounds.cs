using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Game
{
    public class AllSounds : MonoBehaviour
    {
        [Space(5f)] 
        public AssetReferenceAudioClip[] MusicsAssetReference; 
        public AudioMixerGroup MusicMixerGroup;
        public AudioMixerGroup SfxMixerGroup;
    }
}