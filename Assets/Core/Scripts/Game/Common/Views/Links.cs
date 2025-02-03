using System;
using PathCreation;
using PoolSystem.Alternative;
using UnityEngine;

namespace Client.Game
{
    public class Links : MonoBehaviour
    {
        public Grid Grid;
        public PathCreator PathCreator;
        public CarsConfig CarsConfig;
        
        public static Links Instance => _instance;
        private static Links _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                return;
            }
            Destroy(gameObject);
        }
    }
}