using System;
using LGrid;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Game
{
    public class VehicleBuyer : MonoBehaviour
    {
        public Button Button;
        public TMP_Text Text;
        private int _purchaseNumber = 1;
        private const int DefaultCost = 5;
        private int Cost => DefaultCost * _purchaseNumber * _purchaseNumber;

        private void OnEnable()
        {
            Bank.OnCoinsValueChangedEvent += ButtonState;
        }

        private void OnDisable()
        {
            Bank.OnCoinsValueChangedEvent -= ButtonState;
        }

        private void Start()
        {
            Button.interactable = Bank.HasEnoughCoins(Cost);
            ChangeCostText();
        }

        public void Buy()
        {
            if (Map.Instance.HasFreeCell(out var pair))
            {
                if (!Bank.SpendCoins(this,Cost)) return;
                var vehicle = AllVehicles.Instance.CarsPool[0].GetFromPool(pair.Key);
                pair.Value.TaxiBase = vehicle.GetComponent<TaxiBase>();
                _purchaseNumber++;
                ChangeCostText();    
            }
        }

        private void ChangeCostText()
        {
            Text.text = $"Buy {Cost}";
        }

        private void ButtonState(object o, int i, int arg3)
        {
            Button.interactable = Bank.HasEnoughCoins(Cost);
        }
    }
}