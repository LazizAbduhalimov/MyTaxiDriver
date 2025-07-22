using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;
using UI.Buttons;
using UnityEngine;
using YG;
using YG.Insides;

namespace Game
{
    public class BonusGiveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<ERewardVideoClicked>> _eRewardVideoClickedFilter;
        private EcsFilterInject<Inc<CRewardVideoButton>> _cRewardVideoButtonFilter;
        
        private EcsPoolInject<EGiveRandomBonus> _eGiveRandomBonus = "events";

        private Tween? _tween;
        
        public void Init(IEcsSystems systems)
        {
            _tween = HideRewardButtonForSeconds(1);
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eRewardVideoClickedFilter.Value)
            {
                YG2.RewardedAdvShow("Random", () =>
                {
                    _eGiveRandomBonus.NewEntity(out _);
                    _tween?.Stop();
                    _tween = HideRewardButtonForSeconds(5); 
                });
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