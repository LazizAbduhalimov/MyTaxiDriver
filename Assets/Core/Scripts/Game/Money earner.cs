using Client.Game;
using Core.Scripts.Game;
using PrimeTween;
using UnityEngine;

public class Moneyearner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collector>(out var taxi))
        {
            taxi.TaxiBase.Earn();
            var position = taxi.transform.position;
            SoundManager.Instance.PlayFX(AllSfxSounds.Earned, position);
            var poolObject = Links.Instance.PopupsPool.GetFromPool(position.AddY(10f)) as PopupCoin;
            if (poolObject != null)
            {
                poolObject.Text.text = $"+{taxi.TaxiBase.MoneyForCircle}";
                Tween.LocalPositionY(poolObject.transform, poolObject.transform.position.y + 5f, duration: 0.5f, Ease.OutSine)
                    .OnComplete(() => poolObject.gameObject.SetActive(false));
            }
        }
    }
}
