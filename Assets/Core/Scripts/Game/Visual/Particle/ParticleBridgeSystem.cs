using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class ParticleBridgeSystem : IEcsRunSystem
    {
        private EcsCustomInject<AllPools> _allPools;
        private EcsFilterInject<Inc<EMerged>> _eMergedFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eMergedFilter.Value) PlayMergeEffect(entity);
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
    }
}