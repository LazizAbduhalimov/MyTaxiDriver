using Game.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game
{
    public class ParticleBridgeSystem : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        private EcsFilterInject<Inc<EMerged>> _eMergedFilter = "events";
        private EcsFilterInject<Inc<ECarOccured>> _eCarOccuredFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eMergedFilter.Value) PlayMergeEffect(entity);
            foreach (var entity in _eCarOccuredFilter.Value) PlayCarOccurEffect(entity);
        }

        private void PlayMergeEffect(int entity)
        {
            ref var mergedData = ref _eMergedFilter.Pools.Inc1.Get(entity);
            var source = mergedData.Source;
            var target = mergedData.Target;
            _allPools.Value.MergeEffect.GetFromPool(source.Follower.transform.position);
            _allPools.Value.MergeEffect.GetFromPool(target.Follower.transform.position);
            _allPools.Value.MergeEffect.GetFromPool(target.TransparentGfx.transform.position);
        }

        private void PlayCarOccurEffect(int entity)
        {
            var taxiMb = _eCarOccuredFilter.Pools.Inc1.Get(entity).TaxiMb;
            _allPools.Value.MergeEffect.GetFromPool(taxiMb.TransparentGfx.position);
        }
    }
}