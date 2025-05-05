using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] Spider Body;
    [SerializeField] float speed;
    [SerializeField] float stepDistance;
    [SerializeField] float stepHeight;
    [SerializeField] Vector3 footOffset = default;
    [HideInInspector] public float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    private void Start()
    {
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        var body = Body.transform;
        transform.position = currentPosition;
        transform.up = currentNormal;
        var rayOrigin = Vector3.up/2 + body.position + body.right * footSpacing + body.TransformDirection(footOffset);
        Debug.DrawLine(body.position, rayOrigin, Color.green);

        if (lerp < 1)
        {
            var tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
        
            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        if (lerp >= 1)
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    public void ForceUpdateStep()
    {
        var body = Body.transform;
        var rayOrigin = Vector3.up/2 + body.position + body.right * footSpacing + body.TransformDirection(footOffset);
        var ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out var info, 10, terrainLayer.value) && lerp >= 1)
        {
            var distance = Vector3.Distance(newPosition, info.point);
            if (distance > stepDistance)
            {
                lerp = 0;
                newPosition = info.point;
                newNormal = info.normal;
            }
        }
    }

    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(newPosition, 0.25f);   
        }
    }

    public bool IsMoving() => lerp < 1;

    private void OnDrawGizmosSelected()
    {
        if (Body == null) return;
        if (Application.isPlaying) return;
        var position = Body.transform.position + Body.transform.right * footSpacing + footOffset;
        Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(, stepDistance);
        ExtendedGizmos.DrawCircle(position, stepDistance);
        ExtendedGizmos.DrawEllipse(position, stepDistance*2, stepHeight * 2, plane: ExtendedGizmos.Plane.XY);
    }
}
