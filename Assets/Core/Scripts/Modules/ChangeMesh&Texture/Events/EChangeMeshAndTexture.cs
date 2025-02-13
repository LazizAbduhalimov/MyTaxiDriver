using UnityEngine;

namespace Module.Events
{
    public struct EChangeMeshAndTexture
    {
        public GameObject GameObject;
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public Mesh Mesh;
        public Material[] Materials;

        public void Invoke(GameObject gameObject, Mesh mesh, Material[] materials)
        {
            MeshFilter = gameObject.GetComponent<MeshFilter>();
            MeshRenderer = gameObject.GetComponent<MeshRenderer>();
            Mesh = mesh;
            Materials = materials;
        }
    }
}