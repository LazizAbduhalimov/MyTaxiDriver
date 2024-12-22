using System;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client.Game
{
    public class TaxiBase : MouseHoldCount
    {
        public bool IsDriving => _isDriving;
        public int Level;
        public float Speed;
        public int MoneyForCircle;
        
        private PathFollower _follower;
        private Transform _transparentGfx;
        private bool _isDriving;

        public void Configurate(CarsConfig carsConfig)
        {
            Speed = carsConfig.GetSpeed(Level);
            MoneyForCircle = (int)carsConfig.GetProfit(Level);
        }

        private void Start()
        {
            _follower = GetComponentInChildren<PathFollower>(true);
            _transparentGfx = GetComponentInChildren<TransparentGFX>(true).transform;
            _follower.speed = Speed;
            _follower.pathCreator = Links.Instance.PathCreator;
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
            _follower.distanceTravelled = 0;
            _isDriving = !_isDriving;
            _follower.enabled = _isDriving;
            _transparentGfx.gameObject.SetActive(_isDriving);
            _follower.GetComponent<BoxCollider>().enabled = _isDriving;
            if (!_isDriving)
            {
                _follower.transform.localPosition = new Vector3(2, 0 ,4);
                _follower.transform.localRotation = Quaternion.identity;
            }
        }
    }
}