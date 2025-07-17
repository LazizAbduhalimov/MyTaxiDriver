using Assets.SimpleLocalization.Scripts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Module.Bank;
using UI.Buttons;
using UnityEngine;

namespace Game
{
    public class VehiclePurchaseButtonStateHandleSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsCustomInject<GameData> _gameData;
        private EcsFilterInject<Inc<EBankValueChanged>> _eBankValueChanged = "events";
        private EcsFilterInject<Inc<CBuyVehicle>> _cBuyVehicleButtonFilter;
        private readonly Color _fadeRed = new (1, 0, 0, 0.3f);
        private string _buyLocalized = "Buy";
        
        public void Init(IEcsSystems systems)
        {
            ChangeBuyText();
            LocalizationManager.OnLocalizationChanged += ChangeBuyText;
            foreach (var buttonEntity in _cBuyVehicleButtonFilter.Value)
            {
                ref var buyButton = ref _cBuyVehicleButtonFilter.Pools.Inc1.Get(buttonEntity);
                ChangeBuyText(in buyButton);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var buttonEntity in _cBuyVehicleButtonFilter.Value)
            {
                ref var buyButton = ref _cBuyVehicleButtonFilter.Pools.Inc1.Get(buttonEntity);
                foreach (var entity in _eBankValueChanged.Value) ChangeBuyText(in  buyButton);
            }
        }
        
        public void Destroy(IEcsSystems systems)
        {
            LocalizationManager.OnLocalizationChanged -= ChangeBuyText;
        }
        
        private void ChangeBuyText(in CBuyVehicle buyButton)
        {
            var cost = _gameData.Value.GetVehicleCost();
            var hasCoins = Bank.HasEnoughCoins(cost);
            buyButton.Handler.Button.interactable = hasCoins;
            buyButton.Text.text = $"{_buyLocalized} {cost}";
            buyButton.Text.color = hasCoins ? Color.white : _fadeRed;
        }

        private void ChangeBuyText()
        {
            _buyLocalized = LocalizationManager.Localize("Menu.Buy");
            foreach (var buttonEntity in _cBuyVehicleButtonFilter.Value)
            {
                ref var buyButton = ref _cBuyVehicleButtonFilter.Pools.Inc1.Get(buttonEntity);
                ChangeBuyText(in buyButton);
            }
        }
    }
}