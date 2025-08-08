using UnityEngine;

public class HighlightPlaceMb : MonoBehaviour
{
    public MeshRenderer Mesh;
    public Color HighlightColor;
    
    public Material DefaultMaterial;
    public Material HighlightMaterial;

    public void Highlight()
    {
        Mesh.material = HighlightMaterial;
    }
    
    public void DisableHighlight()
    {
        Mesh.material = DefaultMaterial;
    }
}
