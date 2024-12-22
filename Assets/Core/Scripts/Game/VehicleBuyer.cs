using System;
using LGrid;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Game
{
    public class VehicleBuyer : MonoBehaviour
    {
        public Button Button;
        public TMP_Text Text;
        private int Cost => DefaultCost * _purchaseNumber * _purchaseNumber;
        private int _purchaseNumber = 1;
        private const int DefaultCost = 5;
        private Sequence? _sequence;

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
                SoundManager.Instance.PlayUISound(AllUiSounds.Purchased, pitchRange: 0.1f);
                _sequence?.Complete();
                _sequence = Sequence.Create(2, CycleMode.Yoyo, Ease.InOutSine)
                    .Chain(Tween.Scale(Button.transform, 1.2f, duration: 0.1f));
                Button.interactable = Bank.HasEnoughCoins(Cost);
            }
        }

        private void ChangeCostText()
        {
            Text.text = $"Buy {Cost}";
        }

        private void ButtonState(object o, int i, int arg3)
        {
            Button.interactable = Bank.HasEnoughCoins(Cost);
            var color = Color.white;
            color.a = 0.3f;
            Text.color = Button.interactable ? Color.white : color;
        }
    }
}