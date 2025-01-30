using System;
using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LSound;
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
                SoundManager.Instance.PlayUISound(AllUiSounds.Purchased, pitchRange: 0.1f);
            }
        }
    }
}