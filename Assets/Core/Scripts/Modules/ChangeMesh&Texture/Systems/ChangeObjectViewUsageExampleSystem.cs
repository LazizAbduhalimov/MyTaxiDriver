using System.Collections.Generic;
using LAddressables;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Module.Events;
using UnityEngine;

namespace Module.Systems
{
    public class ChangeObjectViewUsageExampleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPoolInject<EChangeMesh> _eChangeMesh = "events";
        private EcsPoolInject<EChangeTexture> _eChangeTexture = "events";
        private AssetsLink AssetsLink;
        private int _meshIndex = 0;
        private int _materialIndex;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private MeshAssetReference _lastMesh;
        private MaterialAssetReference[] _lastMaterials;
        
        public void Init(IEcsSystems systems)
        {
            AssetsLink = Object.FindObjectOfType<AssetsLink>();
            _meshFilter = AssetsLink.GameObject.GetComponent<MeshFilter>();
            _meshRenderer = AssetsLink.GameObject.GetComponent<MeshRenderer>();
        }
        
        public void Run(IEcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                NextMesh();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                NextMaterial();
            }
        }

        private async void NextMesh()
        {
            _meshIndex++;
            _materialIndex = 0;
            if (_meshIndex >= AssetsLink.MeshAndMaterials.Length)
            {
                _meshIndex = 0;
            }
            NextMaterial();
            var meshAndMaterial = AssetsLink.MeshAndMaterials[_meshIndex];
            var meshAsset = meshAndMaterial.Mesh;
            var mesh = await AddressableUtility.LoadAssetAsync<Mesh>(meshAsset);
            _eChangeMesh.NewEntity(out _).Invoke(_meshFilter, mesh);
        }

        private async void NextMaterial()
        {
            var materials = new List<Material>();
            var meshAndMaterial = AssetsLink.MeshAndMaterials[_meshIndex];
            var materialsAssets = meshAndMaterial.MaterialVariants[_materialIndex];
            foreach (var materialsAsset in materialsAssets.MaterialsReferences)
            {
                materials.Add(await AddressableUtility.LoadAssetAsync<Material>(materialsAsset));
            }
            _eChangeTexture.NewEntity(out _).Invoke(_meshRenderer, materials.ToArray());
            _materialIndex++;
            if (_materialIndex >= AssetsLink.MeshAndMaterials[_meshIndex].MaterialVariants.Length)
            {
                _materialIndex = 0;
            }
        }
    }
}