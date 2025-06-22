using PathCreation;
using UnityEngine;

namespace Game.Game
{
    public class Links : MonoBehaviour
    {
        public Grid Grid;
        public PathCreator PathCreator;
        
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