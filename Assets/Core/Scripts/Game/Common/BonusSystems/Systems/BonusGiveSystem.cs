using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;
using UI;
using UI.Buttons;
using UnityEngine;
using YG;

namespace Game
{
    public class BonusGiveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<CInterface>> _cInterfaceFilter;
        private EcsFilterInject<Inc<ERewardVideoClicked>> _eRewardVideoClickedFilter;
        private EcsFilterInject<Inc<ETwoBonusesVideoClicked>> _eTwoBonusesVideoClicked;
        private EcsFilterInject<Inc<CRewardVideoButton>> _cRewardVideoButtonFilter;
        
        private EcsPoolInject<EGiveRandomBonus> _eGiveRandomBonus = "events";
        private EcsPoolInject<EBoostAllCarsBonus> _eBoostAllCarsBonus = "events";
        private EcsPoolInject<EDoubledCoinsBonus> _eDoubledCoinsBonus = "events";

        private Sequence? _sequence;
        
        public void Init(IEcsSystems systems)
        {
            RestartRewardTween(1);
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eRewardVideoClickedFilter.Value)
            {
                YG2.RewardedAdvShow("Random", () =>
                {
                    _eGiveRandomBonus.NewEntity(out _);
                    RestartRewardTween(15);
                });
            }

            foreach (var entity in _eTwoBonusesVideoClicked.Value)
            {
                YG2.RewardedAdvShow("Random", () =>
                {
                    _eBoostAllCarsBonus.NewEntity(out _);
                    _eDoubledCoinsBonus.NewEntity(out _);
                    RestartRewardTween(15);
                });
            }
        }

        private void RestartRewardTween(float hideSeconds)
        {
            _sequence?.Stop();
            _sequence = Sequence.Create()
                    .Chain(HideRewardButtonForSeconds(hideSeconds)
                    .Chain(Tween.Delay(duration: 20f, ShowTwoBonuses)))
                    ;
        }

        private void ShowTwoBonuses()
        {
            foreach (var entity in _cInterfaceFilter.Value)
            {
                ref var ui  = ref _cInterfaceFilter.Pools.Inc1.Get(entity);
                ui.TwoBonusesButtonParent.gameObject.SetActive(true);
                var parent = ui.TwoBonusesButtonInnerParent;
                parent.localScale = Vector3.zero;
                Tween.Scale(parent, 1, .25f, Ease.InSine);
            }
        }

        private Tween HideRewardButtonForSeconds(float second)
        {
            SetActiveRewardVideo(false);
            return Tween.Delay(second, () => SetActiveRewardVideo(true));
        }

        private void SetActiveRewardVideo(bool isActive)
        {
            foreach (var entity in _cRewardVideoButtonFilter.Value)
            {
                ref var btn = ref _cRewardVideoButtonFilter.Pools.Inc1.Get(entity);
                btn.Handler.Button.gameObject.SetActive(isActive);
            }
        }
    }
}