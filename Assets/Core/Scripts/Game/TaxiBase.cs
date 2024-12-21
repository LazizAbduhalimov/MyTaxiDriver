using System;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client.Game
{
    public class TaxiBase : MouseHoldCount
    {
        [FormerlySerializedAs("OpaqueGFX")] public PathFollower Follower;
        public Transform TransparentGFX;
        
        public int Level;
        public float Speed;
        public int MoneyForCircle;
        
        public bool IsDriving;
        
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
            IsDriving = !IsDriving;
            Follower.enabled = IsDriving;
            TransparentGFX.gameObject.SetActive(IsDriving);
            Follower.GetComponent<BoxCollider>().enabled = IsDriving;
            
            if (!IsDriving)
            {
                Follower.transform.localPosition = new Vector3(2, 0 ,4);
                Follower.transform.localRotation = Quaternion.identity;
            }
        }
    }
}