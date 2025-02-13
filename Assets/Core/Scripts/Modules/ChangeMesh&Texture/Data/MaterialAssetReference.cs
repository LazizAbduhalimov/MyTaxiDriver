using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Module
{
    [Serializable]
    public class MaterialAssetReference : AssetReferenceT<Material>
    {
        public MaterialAssetReference(string guid) : base(guid) { }
    }
}