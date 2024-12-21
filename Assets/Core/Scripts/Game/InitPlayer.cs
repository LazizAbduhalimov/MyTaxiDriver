using System;
using UnityEngine;

namespace Client.Game
{
    public class InitPlayer : MonoBehaviour
    {
        public void Start()
        {
            Bank.AddCoins(this, 5);
        }
    }
}