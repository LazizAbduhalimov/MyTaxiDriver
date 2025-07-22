using System;
using PoolSystem.Alternative;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BoostMb : PoolObject
    {
        public Image Timer;
        public Image Icon;
        
        public float LastSetDuration { get; private set; }
        public float DurationLeft { get; private set; }

        private void Update()
        {
            DurationLeft -= Time.deltaTime;
            var fillAmount = DurationLeft / LastSetDuration;
            Timer.fillAmount = fillAmount;
            if (DurationLeft <= 0)
            {
                gameObject.SetActive(false);
                DurationLeft = 0;
                Icon.sprite = null;
            }
        }

        public void Setup(Sprite icon, float duration)
        {
            LastSetDuration = duration;
            DurationLeft = duration;
            Icon.sprite = icon;
        }
    }
}
