using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LSound;
using PrimeTween;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Game
{
    public class MusicBridgeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<CMusic>, Exc<CPlayed>> _unPlayedMusics;
        private EcsFilterInject<Inc<CMusic, CPlayed>> _playedMusics;
        
        private EcsPoolInject<CMusic> _cMusic;
        private EcsPoolInject<CPlayed> _cPlayed;
        private EcsPoolInject<PlayMusic> _ePlayMusic = "events";
        
        private AllSounds _allSounds;
        private Tween? _tween;
        
        public void Init(IEcsSystems systems)
        {
            _allSounds = Object.FindObjectOfType<AllSounds>();
            var random = new Random();
            var musics = _allSounds.Musics.OrderBy(x => random.Next()).ToArray();
            foreach (var music in musics) _cMusic.NewEntity(out _).Clip = music;
            PlayRandomMusic();
        }

        private void PlayRandomMusic()
        {
            float musicLength = int.MaxValue;
            foreach (var entity in _unPlayedMusics.Value)
            {
                ref var music = ref _cMusic.Value.Get(entity);
                _ePlayMusic.NewEntity(out _).Invoke(music.Clip, _allSounds.MusicMixerGroup);
                musicLength = music.Clip.length;
                _cPlayed.Value.Add(entity);
                break;
            }

            if (_unPlayedMusics.Value.GetEntitiesCount() == 0)
            {
                foreach (var entity in _playedMusics.Value) _cPlayed.Value.Del(entity);
            }
            _tween?.Stop();
            _tween = Tween.Delay(musicLength, PlayRandomMusic);
        }

        public void Run(IEcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                PlayRandomMusic();
            }
        }
    }
}