using System;
using PathCreation.Examples;
using UnityEngine;

namespace Client.Game
{
    public class TaxiBase : MouseHoldCount
    {
        public Action<bool> OnDrivingStateChange;
        public bool IsDriving => _isDriving;
        public int Level;
        public float Speed;
        public int MoneyForCircle;

        [HideInInspector] public PathFollower Follower;
        private Transform _transparentGfx;
        private bool _isDriving;
        private Vector3 _defaultOffset;

        public void Configurate(CarsConfig carsConfig)
        {
            Speed = carsConfig.GetSpeed(Level);
            MoneyForCircle = (int)carsConfig.GetProfit(Level);
        }

        private void Start()
        {
            Follower = GetComponentInChildren<PathFollower>(true);
            _transparentGfx = GetComponentInChildren<TransparentGFX>(true).transform;
            Follower.speed = Speed;
            Follower.pathCreator = Links.Instance.PathCreator;
            _defaultOffset = _transparentGfx.localPosition;
            Debug.Log(_defaultOffset);
        }

        public virtual void Earn()
        {
            Bank.AddCoins(this, MoneyForCircle);
        }

        protected override void HandleClick()
        {
            ChangeState();
        }

        private void ChangeState()
        {
            Follower.distanceTravelled = 0;
            _isDriving = !_isDriving;
            Follower.enabled = _isDriving;
            _transparentGfx.gameObject.SetActive(_isDriving);
            Follower.GetComponent<BoxCollider>().enabled = _isDriving;
            if (!_isDriving)
            {
                Follower.transform.localPosition = _defaultOffset;
                Follower.transform.localRotation = Quaternion.identity;
            }
            
            var sound = _isDriving ? AllSfxSounds.ToPark : AllSfxSounds.BackToPark;
            SoundManager.Instance.PlayFX(sound, transform.position);
            OnDrivingStateChange?.Invoke(_isDriving);
        }
    }
}