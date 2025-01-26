using System.Linq;
using LAddressables;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LSound;
using PrimeTween;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        private AssetReference _assetReference;
        
        public void Init(IEcsSystems systems)
        {
            _allSounds = Object.FindObjectOfType<AllSounds>();
            var random = new Random();
            var musics = _allSounds.MusicsAssetReference.OrderBy(x => random.Next()).ToArray();
            foreach (var referenceAudioClip in musics) _cMusic.NewEntity(out _).ReferenceAudioClip = referenceAudioClip;
            PlayRandomMusic();
        }

        private async void PlayRandomMusic()
        {
            float musicLength = int.MaxValue;
            foreach (var entity in _unPlayedMusics.Value)
            {
                if (_assetReference != null)
                    AddressableUtility.Release(_assetReference);
                _assetReference = _cMusic.Value.Get(entity).ReferenceAudioClip;
                await AddressableUtility.LoadAssetAsync<AudioClip>("music_warm");
                await AddressableUtility.LoadAssetAsync<AudioClip>("music_warm");
                var task = AddressableUtility.LoadAssetAsync<AudioClip>(_assetReference);
                await task;
                _ePlayMusic.NewEntity(out _).Invoke(task.Result, _allSounds.MusicMixerGroup);
                musicLength = task.Result.length;
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