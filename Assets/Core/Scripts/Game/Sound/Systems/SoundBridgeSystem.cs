using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LSound;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class SoundBridgeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AllSounds _allSounds;
        
        public void Init(IEcsSystems systems)
        {
            _allSounds = Object.FindObjectOfType<AllSounds>();
        }
        
        public void Run(IEcsSystems systems)
        {
         
        }
    }
}