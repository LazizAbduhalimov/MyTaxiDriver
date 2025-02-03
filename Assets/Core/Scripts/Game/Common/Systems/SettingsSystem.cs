using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Game
{
    public class SettingsSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            Application.targetFrameRate = 120;
        }
    }
}