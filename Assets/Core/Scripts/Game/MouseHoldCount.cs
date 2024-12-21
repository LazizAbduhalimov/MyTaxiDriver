using System;
using UnityEngine;

namespace Client.Game
{
    public class MouseHoldCount : MonoBehaviour
    {
        [SerializeField] protected float SleepTime;
        protected float PassedTime;
        protected bool ConditionVerified;

        protected void OnMouseDown()
        {
            HandleDown();
        }

        protected void OnMouseDrag()
        {
            ConditionVerified = ShouldHandleClick();
            HandleDrag();
        }

        protected void OnMouseUp()
        {
            if (ShouldHandleClick())
            {
                HandleClick();
            }

            PassedTime = 0;
        }

        protected virtual bool ShouldHandleClick()
        {
            return PassedTime < SleepTime;
        }

        protected virtual void HandleClick()
        {
            Debug.Log("Doing Smth");
        }

        protected virtual void HandleDown()
        {
            PassedTime = 0;
        }

        protected virtual void HandleDrag()
        {
            PassedTime += Time.deltaTime;
        }
    }
}