using UnityEngine;
using UnityEngine.Serialization;

public class TargetMover : MonoBehaviour
{
    [FormerlySerializedAs("terrainLayer")] [SerializeField] LayerMask _terrainLayer;
    [FormerlySerializedAs("Body")] [SerializeField] Spider _spider;
    [FormerlySerializedAs("OtherFoot")] [SerializeField] TargetMover _otherFoot;
    [FormerlySerializedAs("speed")] [SerializeField] float _speed;
    [FormerlySerializedAs("stepDistance")] [SerializeField] float _stepDistance;
    [FormerlySerializedAs("stepHeight")] [SerializeField] float _stepHeight;
    [FormerlySerializedAs("footOffset")] [SerializeField] Vector3 _footOffset = default;
    [FormerlySerializedAs("footSpacing")] [HideInInspector] public float FootSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    
    private float _lerp;

    private void Start()
    {
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        _lerp = 1;
    }

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;
        RepositionTarget();

        if (_lerp < 1 && !_otherFoot.IsMoving())
        {
            LerpLegPosition();
        }
        
        if (_lerp >= 1)
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void LerpLegPosition()
    {
        var tempPosition = Vector3.Lerp(oldPosition, newPosition, _lerp);
        tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;
        
        currentPosition = tempPosition;
        currentNormal = Vector3.Lerp(oldNormal, newNormal, _lerp);
        _lerp += Time.deltaTime * _speed;
    }

    public void RepositionTarget()
    {
        var body = _spider.transform;
        var rayOrigin = Vector3.up/2 + body.position + body.right * FootSpacing + body.TransformDirection(_footOffset);
        var ray = new Ray(rayOrigin, Vector3.down);
        Debug.DrawLine(body.position, rayOrigin, Color.green);
        
        if (_lerp >= 1 && !_otherFoot.IsMoving() &&
            Physics.Raycast(ray, out var info, 10, _terrainLayer.value))
        {
            var distance = Vector3.Distance(newPosition, info.point);
            if (distance > _stepDistance)
            {
                _lerp = 0;
                var direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + body.forward * (direction * _stepDistance);
                newNormal = info.normal;
            }
        }
    }

    public void ForceRepositionTarget()
    {
        var body = _spider.transform;
        var rayOrigin = Vector3.up/2 + body.position + body.right * FootSpacing + body.TransformDirection(_footOffset);
        var ray = new Ray(rayOrigin, Vector3.down);
        Debug.DrawLine(body.position, rayOrigin, Color.green);
        
        if (Physics.Raycast(ray, out var info, 10, _terrainLayer.value))
        {
            _lerp = 0;
            newPosition = info.point;
            newNormal = info.normal;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.25f);
    }

    public bool IsMoving() => _lerp < 1;

    private void OnDrawGizmosSelected()
    {
        if (_spider == null) return;
        if (Application.isPlaying) return;
        var position = _spider.transform.position + _spider.transform.right * FootSpacing + _footOffset;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(position, 0.1f);
        ExtendedGizmos.DrawCircle(position, _stepDistance);
        ExtendedGizmos.DrawEllipse(position, _stepDistance*2, _stepHeight * 2, plane: ExtendedGizmos.Plane.XY);
    }
}
