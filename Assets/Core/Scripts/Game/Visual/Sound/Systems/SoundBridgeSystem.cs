using Client;
using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UI.Buttons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class SoundBridgeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AllSounds _allSounds;
        private EcsFilterInject<Inc<EBuyVehicleClicked>> _eBuyVehicleClickedFilter;
        private EcsFilterInject<Inc<EEarnMoney>> _eEarnMoneyFilter = "events";
        private EcsFilterInject<Inc<EMerged>> _eMergedFilter = "events";
        
        public void Init(IEcsSystems systems)
        {
            _allSounds = Object.FindObjectOfType<AllSounds>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eEarnMoneyFilter.Value)
            {
                SoundManager.Instance.PlayFX(AllSfxSounds.Earned);
            }

            foreach (var entity in _eBuyVehicleClickedFilter.Value)
            {
                
            }

            foreach (var entity in _eMergedFilter.Value) PlayMergeSound(entity);
        }

        private void PlayMergeSound(int entity)
        {
            ref var merged = ref _eMergedFilter.Pools.Inc1.Get(entity);
            SoundManager.Instance.PlayFX(AllSfxSounds.Merge, merged.Target.transform.position);
        }
    }
}