using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UI.Buttons;

namespace Client
{
    public class BonusGiveSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<ERewardVideoClicked>> _eRewardVideoClickedFilter;
        private EcsPoolInject<EGiveRandomBonus> _eGiveRandomBonus = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eRewardVideoClickedFilter.Value)
            {
                _eGiveRandomBonus.NewEntity(out _);
            }
        }
    }
}