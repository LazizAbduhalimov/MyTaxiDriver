using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace UI
{
    public class InitUIInterface : IEcsInitSystem
    {
        private EcsPoolInject<CInterface> _cInterface;
            
        public void Init(IEcsSystems systems)
        {
            ref var cInterface = ref _cInterface.NewEntity(out _);
            var ui = Object.FindObjectOfType<UILinks>();
            cInterface.BuyVehicleButton = ui.BuyVehicleButton;
            cInterface.BuyVehicleCostText = ui.BuyVehicleCostText;
        }
    }
}