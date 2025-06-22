using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class BonusHandleSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<CInterface>> _cInterfaceFilter;
        private EcsFilterInject<Inc<EGiveRandomBonus>> _eGiveRandomBonusFilter = "events";
        private EcsPoolInject<EBonusCar> _eBonusCar = "events";
        private EcsPoolInject<EBonusCoins> _eBonusCoins = "events";
        private EcsPoolInject<EBoostAllCarsBonus> _eBoostAllCarsBonus = "events";
        private EcsPoolInject<EDoubledCoinsBonus> _eDoubledCoinsBonus = "events";
        
        private readonly Dictionary<Type, string> _bonusInfos = new()
        {
            { typeof(EBonusCar), "New Car!"},
            { typeof(EBonusCoins), "You Got Coins!"},
            { typeof(EDoubledCoinsBonus), "2X Coins!"},
            { typeof(EBoostAllCarsBonus), "All Cars Boosted!"},
        };

        private Tween? _tween;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eGiveRandomBonusFilter.Value)
            {
                var r = Random.Range(1, 5);
                switch (r)
                {
                    case 1:
                        _eBonusCar.NewEntity(out _);
                        ShowBonusInfo(typeof(EBonusCar));
                        break;
                    case 2:
                        _eBonusCoins.NewEntity(out _);
                        ShowBonusInfo(typeof(EBonusCoins));
                        break;
                    case 3:
                        _eDoubledCoinsBonus.NewEntity(out _);
                        ShowBonusInfo(typeof(EDoubledCoinsBonus));
                        break;
                    case 4:
                        _eBoostAllCarsBonus.NewEntity(out _);
                        ShowBonusInfo(typeof(EBoostAllCarsBonus));
                        break;
                }
                
                Debug.Log($"Bonus {r}");
                _eGiveRandomBonusFilter.Pools.Inc1.Del(entity);
            }
        }
        
        private void ShowBonusInfo(Type type)
        {
            Debug.Log("Show");
            var bonusText = $"<b>{_bonusInfos[type]}<b>\n<i><size=60>bonus</size></i>";
            foreach (var entity in _cInterfaceFilter.Value)
            {
                var text = _cInterfaceFilter.Pools.Inc1.Get(entity).BonusText;
                text.text = bonusText;
                text.gameObject.SetActive(true);
                
                _tween?.Complete();
                var rectInitial = text.rectTransform.localPosition;
                _tween = Tween.Custom(0, 100, duration: 2,
                    value => text.rectTransform.localPosition = rectInitial.AddY(value))
                        .OnComplete(() => {
                            text.gameObject.SetActive(false);
                            text.rectTransform.localPosition = rectInitial;
                        });
            }
        }
    }
}