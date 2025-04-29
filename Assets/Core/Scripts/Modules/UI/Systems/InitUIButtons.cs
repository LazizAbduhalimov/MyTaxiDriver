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
        private EcsPoolInject<CWatchVideo> _cWatchVideo;
        
        public void Init(IEcsSystems systems)
        {
            foreach (var entity in _cInterfaceFilter.Value)
            {
                ref var ui = ref _cInterfaceFilter.Pools.Inc1.Get(entity);
                ref var buyVehicle = ref UIUtils.InitButton(ui.BuyVehicleButton, _cBuyVehicle.Value);
                buyVehicle.Text = ui.BuyVehicleCostText;
                
                UIUtils.InitButton(ui.RewardVideoButton, _cWatchVideo.Value);
            }
        }
    }
}