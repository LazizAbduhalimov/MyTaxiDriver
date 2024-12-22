using System;
using PathCreation.Examples;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client.Game
{
    public class SpeedBooster : MonoBehaviour
    {
        private Transform _trails;
        private float _boostPercentage = 5f;
        private PathFollower _pathFollower;
        private bool _canBeBoosted = true;
        private float _boostCoolDown = 5f;

        private void Start()
        {
            _pathFollower = GetComponent<PathFollower>();
            _trails = GetComponentsInChildren<Transform>()[1];
            _trails.gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (!_canBeBoosted) return;
            SoundManager.Instance.PlayFX(AllSfxSounds.Woosh, transform.position);
            _trails.gameObject.SetActive(true);
            _canBeBoosted = false;
            var startSpeed = _pathFollower.speed;
            var boostedSpeed = startSpeed + startSpeed * _boostPercentage;
            var sequence = Sequence.Create(cycles: 2, CycleMode.Yoyo, Ease.OutSine)
                .Chain(Tween.Custom(startSpeed, boostedSpeed, duration: 0.5f,
                    value => _pathFollower.speed = value))
                .Chain(Tween.Delay(1f));
            
            sequence.OnComplete(Boost);
        }

        private void Boost()
        {
            Tween.Delay(_boostCoolDown, () => _canBeBoosted = true);
            _trails.gameObject.SetActive(false);
        }
    }
}