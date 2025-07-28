using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;
using UI.Buttons;
using UnityEngine;
using YG;

namespace Game
{
    public class ShowBannerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<EStartMerge>> _eStartMergeFilter = "events";
        private EcsFilterInject<Inc<ERewardVideoClicked>> _eRewardVideoClickedFilter;
        
        private bool _showAdv;
        private const float _advIntervar = 45f;
        private Tween _tween;
        
        public void Init(IEcsSystems systems)
        {
            RestartAdvInterval(_advIntervar);
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eRewardVideoClickedFilter.Value)
            {
                _showAdv = false;
                _tween.Stop();
                RestartAdvInterval(_advIntervar);
            }
            
            foreach (var entity in _eStartMergeFilter.Value)
            {
                if (!_showAdv) return;
                Tween.Delay(0.1f, YG2.InterstitialAdvShow);
                _showAdv = false;
                RestartAdvInterval(_advIntervar);
            }
        }

        private void RestartAdvInterval(float duration)
        {
            Debug.Log($"RestartAdvInterval ({duration} seconds)");
            _tween.Stop();
            _tween = Tween.Delay(duration, () =>
            {
                _showAdv = true;
                Debug.Log("Adv will be showed next merge");
            });
        }
    }
}