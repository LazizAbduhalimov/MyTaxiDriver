using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LSound;
using UnityEngine;

namespace Game
{
    public class MusicSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<CMusicSource>> _cMusicSource;
        private EcsFilterInject<Inc<PlayMusic>> _ePlayMusicFilter = "events";
        
        private EcsPoolInject<CMusicSource> _cMusic;
        
        public void Init(IEcsSystems systems)
        {
            var sound = Object.FindObjectOfType<SoundRefs>();
            var source = Object.Instantiate(sound.SoundSourceObject.AudioSource);
            _cMusic.NewEntity(out _).AudioSource = source;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _ePlayMusicFilter.Value)
            {
                ref var music = ref _ePlayMusicFilter.Pools.Inc1.Get(entity);
                foreach (var sourceEntity in _cMusicSource.Value)
                {
                    ref var musicSource = ref _cMusicSource.Pools.Inc1.Get(sourceEntity);
                    musicSource.AudioSource.Stop();
                    musicSource.AudioSource.outputAudioMixerGroup = music.MixerGroup;
                    musicSource.AudioSource.clip = music.Clip;
                    musicSource.AudioSource.Play();
                }
            }  
        }
    }
}