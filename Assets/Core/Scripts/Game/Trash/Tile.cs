using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Scripts
{
    public class Tile : MonoBehaviour
    {
        [FormerlySerializedAs("Renderer")] public MeshRenderer MeshRenderer;
    }
}