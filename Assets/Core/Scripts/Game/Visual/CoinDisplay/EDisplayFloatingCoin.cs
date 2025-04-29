using UnityEngine;

namespace Client
{
    public struct EDisplayFloatingCoin
    {
        public Vector3 Position;
        public int Value;
        public float Duration;
        
        public void Invoke(Vector3 position, int value, float duration = 0.5f)
        {
            Position = position;
            Value = value;
            Duration = duration;
        }
    }
}