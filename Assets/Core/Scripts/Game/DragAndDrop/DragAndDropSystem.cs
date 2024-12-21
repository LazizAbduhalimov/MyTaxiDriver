using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client.Game.Test
{
    public class DragAndDropSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EDrag>> _eDrag = "events";
        private EcsFilterInject<Inc<EDragStart>> _eDragStart = "events";
        private EcsFilterInject<Inc<EDragEnd>> _eDragEnd = "events";
        
        public void Run(IEcsSystems systems)
        {
            
        }
    }
}