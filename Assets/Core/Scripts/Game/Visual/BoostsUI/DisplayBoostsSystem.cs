using System;
using System.Collections.Generic;
using Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.UI
{
    public class DisplayBoostsSystem : IEcsInitSystem, IEcsRunSystem 
    {
        private EcsFilterInject<Inc<EBoostAllCarsBonus>> _eBoostAllCarsBonusFilter = "events";
        private EcsFilterInject<Inc<EDoubledCoinsBonus>> _eDoubledCoinsBonusFilter = "events";
        
        private Dictionary<Type, Sprite> _boostSprites = new();
        private BoostsHolder _boostsHolder;
        private BonusesData BonusesData => GameData.Instance.BonusesData;
        
        public async void Init(IEcsSystems systems)
        {
            var boostsSprites = Object.FindObjectOfType<BoostsUI>();
            _boostsHolder = Object.FindObjectOfType<BoostsHolder>();
            _boostSprites.Add(typeof(EDoubledCoinsBonus), boostsSprites.DoubleCoins);
            _boostSprites.Add(typeof(EBoostAllCarsBonus), boostsSprites.SpeedBoost);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBoostAllCarsBonusFilter.Value) AddBoost(typeof(EBoostAllCarsBonus));
            foreach (var entity in _eDoubledCoinsBonusFilter.Value) AddBoost(typeof(EDoubledCoinsBonus));
        }

        private void AddBoost(Type bonusType)
        {
            var icon =  _boostSprites[bonusType];
            float duration = 0;
            if (bonusType == typeof(EDoubledCoinsBonus))
            {
                duration = BonusesData.DoubledCoinsDuration;
            }
            else if (bonusType == typeof(EBoostAllCarsBonus))
            {
                duration = BonusesData.SpeedBoostDuration;
            }

            _boostsHolder.SetBoost(icon, duration, bonusType.ToString());
        }
    }
}