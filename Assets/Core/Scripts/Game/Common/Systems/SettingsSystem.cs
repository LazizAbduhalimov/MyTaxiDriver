using Leopotam.EcsLite;
using UnityEngine;
using YG;

namespace Game.Game
{
    public class SettingsSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            Application.targetFrameRate = 120;
            YG2.InterstitialAdvShow();
        }
    }
}