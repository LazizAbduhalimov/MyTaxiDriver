using PathCreation.Examples;
using PrimeTween;
using UnityEngine;

namespace Client.Game
{
    public class SpeedBooster : MonoBehaviour
    {
        private TaxiMb _taxiMb;
        public GameObject Particle;
        private Transform _trails;
        private float _boostPercentage = 5f;
        private PathFollower _pathFollower;
        private bool _canBeBoosted = true;
        private float _boostCoolDown = 5f;
        private Tween? _tween;

        private void OnEnable()
        {
            if (_taxiMb == null) _taxiMb = GetComponentInParent<TaxiMb>();
        }

        private void Start()
        {
            _taxiMb = GetComponentInParent<TaxiMb>();
            _pathFollower = GetComponent<PathFollower>();
            _trails = GetComponentsInChildren<Transform>()[1];
            _trails.gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (!_canBeBoosted) return;
            SetBoostAccessState(false);
            _trails.gameObject.SetActive(true);
            SoundManager.Instance.PlayFX(AllSfxSounds.Woosh, transform.position);
            
            var startSpeed = _pathFollower.speed;
            var boostedSpeed = startSpeed + startSpeed * _boostPercentage;
            var sequence = Sequence.Create(cycles: 2, CycleMode.Yoyo, Ease.OutSine)
                .Chain(Tween.Custom(startSpeed, boostedSpeed, duration: 0.5f,
                    value => _pathFollower.speed = value))
                .Chain(Tween.Delay(1f));
            
            sequence.OnComplete(StartCoolDown);
        }

        private void StartCoolDown()
        {
            _tween = Tween.Delay(_boostCoolDown, () => SetBoostAccessState(true));
            _trails.gameObject.SetActive(false);
        }

        public void SetBoostAccessState(bool isActive)
        {
            _canBeBoosted = isActive;
            if (Particle != null)
                Particle.SetActive(isActive);
        }

        private void OnStateChanged(bool isDriving)
        {
            _tween?.Stop();
            SetBoostAccessState(false);
            if (isDriving)
                StartCoolDown();
        }
    }
}