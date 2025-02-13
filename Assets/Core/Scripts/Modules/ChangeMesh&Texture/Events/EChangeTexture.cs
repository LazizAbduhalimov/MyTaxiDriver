using UnityEngine;

namespace Module.Events
{
    public struct EChangeTexture
    {
        public MeshRenderer MeshRenderer;
        public Material[] Materials;

        public void Invoke(MeshRenderer renderer, Material[] materials)
        {
            MeshRenderer = renderer;
            Materials = materials;
        }
    }
}