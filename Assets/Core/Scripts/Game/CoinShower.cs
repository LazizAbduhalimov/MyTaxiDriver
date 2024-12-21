using System;
using TMPro;
using UnityEngine;

namespace Client.Game
{
    public class CoinShower : MonoBehaviour
    {
        public TMP_Text Text;

        private void OnEnable()
        {
            Text.text = Bank.Coins.ToString();
            Bank.OnCoinsValueChangedEvent += ChangeText;
        }

        private void OnDisable()
        {
            Bank.OnCoinsValueChangedEvent -= ChangeText;
        }

        private void ChangeText(object sender, int oldValue, int newValue)
        {
            Text.text = newValue.ToString();
        }
    }
}