using System;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
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
            { typeof(EBonusCar), "UI.NewCar"},
            { typeof(EBonusCoins), "UI.Coins"},
            { typeof(EDoubledCoinsBonus), "UI.2XCoins"},
            { typeof(EBoostAllCarsBonus), "UI.Boost"},
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
                        // ShowBonusInfo(typeof(EBonusCar));
                        ShowBonusInfo(typeof(EBoostAllCarsBonus));
                        break;
                    case 2:
                        _eBonusCoins.NewEntity(out _);
                        // ShowBonusInfo(typeof(EBonusCoins));
                        ShowBonusInfo(typeof(EDoubledCoinsBonus));
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
                
                _eGiveRandomBonusFilter.Pools.Inc1.Del(entity);
            }
        }
        
        private void ShowBonusInfo(Type type)
        {
            var bonusText = GetBonusText(type);
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

        private string GetBonusText(Type bonusType)
        {
            var bonusText = LocalizationManager.Localize(_bonusInfos[bonusType]);
            var bonus = LocalizationManager.Localize("UI.Bonus");
            var text = $"<b>{bonusText}<b>\n<i><size=60>{bonus}</size></i>";
            return text;
        }
    }
}