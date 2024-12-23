using System;
using LGrid;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Client.Game
{
    public class VehicleBuyer : MonoBehaviour
    {
        [HideInInspector] public int PurchaseNumber = 1;
        public Button Button;
        public TMP_Text Text;
        private int Cost => DefaultCost * PurchaseNumber * PurchaseNumber;
        private const int DefaultCost = 5;
        private Sequence? _sequence;
        private int BuyingCarLevel => PurchaseNumber >= 63 ? 1 : 0; 

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
            Debug.Log(PurchaseNumber);
            if (Map.Instance.HasFreeCell(out var pair))
            {
                if (!Bank.SpendCoins(this,Cost)) return;
                var vehicle = AllVehicles.Instance.CarsPool[BuyingCarLevel].GetFromPool(pair.Key);
                pair.Value.TaxiBase = vehicle.GetComponent<TaxiBase>();
                PurchaseNumber++;
                ChangeCostText();    
                SoundManager.Instance.PlayUISound(AllUiSounds.Purchased, pitchRange: 0.1f);
                _sequence?.Complete();
                _sequence = Sequence.Create(2, CycleMode.Yoyo, Ease.InOutSine)
                    .Chain(Tween.Scale(Button.transform, 1.2f, duration: 0.1f));
                Button.interactable = Bank.HasEnoughCoins(Cost);
            }
        }

        public void ChangeCostText()
        {
            Text.text = $"Buy {Cost}";
        }

        private void ButtonState(object o, long i, long arg3)
        {
            Button.interactable = Bank.HasEnoughCoins(Cost);
            var color = Color.white;
            color.a = 0.3f;
            Text.color = Button.interactable ? Color.white : color;
        }
    }
}