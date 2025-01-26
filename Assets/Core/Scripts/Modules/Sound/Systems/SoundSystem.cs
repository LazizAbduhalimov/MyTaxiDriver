using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PoolSystem.Alternative;
using PrimeTween;
using Object = UnityEngine.Object;

namespace LSound
{
    public class SoundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<PoolService> _poolService;
        private EcsFilterInject<Inc<PlaySound>> _ePlaySoundFilter = "events";
        
        private PoolMono<SoundSourceObject> _audioSourcePool;

        public void Init(IEcsSystems systems)
        {
            var sound = Object.FindObjectOfType<SoundRefs>();
            _audioSourcePool = _poolService.Value.GetOrRegisterPool(sound.SoundSourceObject, 10);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _ePlaySoundFilter.Value)
            {
                ref var sound = ref _ePlaySoundFilter.Pools.Inc1.Get(entity);
                var audioSource = _audioSourcePool.GetFreeElement();
                LocateAudioSource(audioSource, in sound);
                audioSource.AudioSource.outputAudioMixerGroup = sound.MixerGroup;
                audioSource.PlayClip(sound.Clip);
                _ePlaySoundFilter.Pools.Inc1.Del(entity);
            }
        }

        private void LocateAudioSource(SoundSourceObject source, in PlaySound sound)
        {
            var transform = source.transform;
            transform.position = sound.Position;
            if (sound.Parent == null) return;
            var parent = source.transform.parent;
            transform.parent = sound.Parent;
            Tween.Delay(sound.Clip.length, () => transform.parent = parent);
        }
    }
}
