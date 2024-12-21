using System;
using PathCreation.Examples;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client.Game
{
    public class SpeedBooster : MonoBehaviour
    {
        private float _boostPercentage = 5f;
        private PathFollower _pathFollower;
        private bool _canBeBoosted = true;
        private float _boostCoolDown = 10f;

        private void Start()
        {
            _pathFollower = GetComponent<PathFollower>();
        }

        private void OnMouseDown()
        {
            if (!_canBeBoosted) return;
            _canBeBoosted = false;
            var startSpeed = _pathFollower.speed;
            var boostedSpeed = startSpeed + startSpeed * _boostPercentage;
            var sequence = Sequence.Create(cycles: 2, CycleMode.Yoyo, Ease.OutSine)
                .Chain(Tween.Custom(startSpeed, boostedSpeed, duration: 1f,
                    value => _pathFollower.speed = value))
                .Chain(Tween.Delay(1.5f));
            
            sequence.OnComplete(
                () => Tween.Delay(_boostCoolDown, () => _canBeBoosted = true));
        }
    }
}