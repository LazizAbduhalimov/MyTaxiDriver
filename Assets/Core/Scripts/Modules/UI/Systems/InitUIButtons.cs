using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UI.Buttons;

namespace UI
{
    public class InitUIButtons : IEcsInitSystem
    {
        private EcsWorldInject _world;
        private EcsFilterInject<Inc<CInterface>> _cInterfaceFilter;

        private EcsPoolInject<CBuyVehicle> _cBuyVehicle;
        private EcsPoolInject<CRewardVideoButton> _cWatchVideo;
        private EcsPoolInject<CTwoBonusesVideo> _cTwoBonusesVideo;
        
        public void Init(IEcsSystems systems)
        {
            foreach (var entity in _cInterfaceFilter.Value)
            {
                ref var ui = ref _cInterfaceFilter.Pools.Inc1.Get(entity);
                ref var buyVehicle = ref UIUtils.InitButton(ui.BuyVehicleButton, _cBuyVehicle.Value);
                buyVehicle.Text = ui.BuyVehicleCostText;
                
                UIUtils.InitButton(ui.RewardVideoButton, _cWatchVideo.Value);
                UIUtils.InitButton(ui.RewardVideoButtonInner, _cWatchVideo.Value);
                ref var button = ref UIUtils.InitButton(ui.TwoBonusesButton, _cTwoBonusesVideo.Value);
                button.Parent = ui.TwoBonusesButtonParent;
            }
        }
    }
}