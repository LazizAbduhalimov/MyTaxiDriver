using System;
using UnityEngine.Serialization;

namespace Module
{
    [Serializable]
    public class MeshAndMaterials
    {
        public MeshAssetReference Mesh;
        public MaterialsAssetsList[] MaterialVariants;
    }

    [Serializable]
    public class MaterialsAssetsList
    {
        public MaterialAssetReference[] MaterialsReferences;
    }
}