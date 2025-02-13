using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Module.Events;
using UnityEngine;

namespace Module
{
    public class ChangeMeshAndTextureSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EChangeMesh>> _eChangeMeshFilter = "events";
        private EcsFilterInject<Inc<EChangeTexture>> _eChangeTextureFilter = "events";
        private EcsFilterInject<Inc<EChangeMeshAndTexture>> _eChangeMeshAndTextureFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eChangeMeshFilter.Value)
            {
                ref var meshData = ref _eChangeMeshFilter.Pools.Inc1.Get(entity);
                meshData.MeshFilter.mesh = meshData.Mesh;
                _eChangeMeshFilter.Pools.Inc1.Del(entity);
            }

            foreach (var entity in _eChangeTextureFilter.Value)
            {
                ref var textureData = ref _eChangeTextureFilter.Pools.Inc1.Get(entity);
                textureData.MeshRenderer.materials = textureData.Materials;
                _eChangeTextureFilter.Pools.Inc1.Del(entity);
            }

            foreach (var entity in _eChangeMeshAndTextureFilter.Value)
            {
                ref var meshData = ref _eChangeMeshAndTextureFilter.Pools.Inc1.Get(entity);
                meshData.MeshFilter.mesh = meshData.Mesh;
                meshData.MeshRenderer.materials = meshData.Materials;
                _eChangeMeshAndTextureFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}