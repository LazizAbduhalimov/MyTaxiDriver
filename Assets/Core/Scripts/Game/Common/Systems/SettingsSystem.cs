using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Game
{
    public class SettingsSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            Application.targetFrameRate = 120;
        }
    }
}