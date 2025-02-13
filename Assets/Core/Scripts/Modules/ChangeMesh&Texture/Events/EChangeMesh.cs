using UnityEngine;

namespace Module.Events
{
    public struct EChangeMesh
    {
        public MeshFilter MeshFilter;
        public Mesh Mesh;

        public void Invoke(MeshFilter meshFilter, Mesh mesh)
        {
            MeshFilter = meshFilter;
            Mesh = mesh;
        }
    }
}