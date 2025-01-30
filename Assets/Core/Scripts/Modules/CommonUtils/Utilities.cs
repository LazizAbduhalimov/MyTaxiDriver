using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    public static class Utilities
    {
        public static EcsWorld World;
        public static EcsWorld EventsWorld;
        
        private static Camera _mainCamera;

        public static void Init(EcsWorld world, EcsWorld eventsWorld)
        {
            World = world;
            EventsWorld = eventsWorld;
            _mainCamera = Camera.main;
        }

        public static int FastUnpack(this EcsPackedEntity packed)
        {
            if (!packed.Unpack(World, out var entity))
                throw new Exception("Can't unpack entity");
            return entity;
        }

        public static ref T UnpackAndGetFromPool<T>(this EcsPackedEntity packed) where T : struct
        {
            return ref World.GetPool<T>().Get(packed.FastUnpack());
        }
        
        public static Vector3 GetMouseWorldPosition()
        {
            var mouseScreenPosition = Input.mousePosition;
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _mainCamera.ScreenPointToRay(mouseScreenPosition);
            return plane.Raycast(ray, out var distance) ? ray.GetPoint(distance) : Vector3.zero;
        }
    }
}