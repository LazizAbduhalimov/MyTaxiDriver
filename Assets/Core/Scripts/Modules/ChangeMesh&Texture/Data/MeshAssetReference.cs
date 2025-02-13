using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Module
{
    [Serializable]
    public class MeshAssetReference : AssetReferenceT<Mesh>
    {
        public MeshAssetReference(string guid) : base(guid) { }
    }
}