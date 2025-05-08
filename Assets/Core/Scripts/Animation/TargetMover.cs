using System;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public Spider Spider => _spider;
    public float StepDistance => _stepDistance;
    public float StepLength => _stepLength;
    public float StepHeight => _stepHeight;
    public Vector3 FootOffset => _footOffset;

    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private Spider _spider;
    [SerializeField] private float _speed;
    [SerializeField] private float _stepDistance;
    [SerializeField] private float _stepLength;
    [SerializeField] private float _stepHeight;
    [SerializeField] private Vector3 _footOffset;

    public Vector3 OldPosition { get; private set; }
    public Vector3 CurrentPosition { get; private set; }
    public Vector3 NewPosition { get; private set; }

    public Vector3 OldNormal { get; private set; }
    public Vector3 CurrentNormal { get; private set; }
    public Vector3 NewNormal { get; private set; }

    private float _lerp;

    private void Start()
    {
        CurrentPosition = NewPosition = OldPosition = transform.position;
        CurrentNormal = NewNormal = OldNormal = transform.up;
        _lerp = 1;
    }

    public void Update()
    {
        transform.position = CurrentPosition;
        transform.up = CurrentNormal;

        if (_lerp < 1)
        {
            LerpLegPosition();
        }

        if (_lerp >= 1)
        {
            OldPosition = NewPosition;
            OldNormal = NewNormal;
        }
    }

    private void LerpLegPosition()
    {
        var tempPosition = Vector3.Lerp(OldPosition, NewPosition, _lerp);
        tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;

        CurrentPosition = tempPosition;
        CurrentNormal = Vector3.Lerp(OldNormal, NewNormal, _lerp);
        _lerp += Time.deltaTime * _speed;
    }

    public void RepositionTarget()
    {
        var body = _spider.transform;
        var rayOrigin = Vector3.up / 2 + body.position + body.TransformDirection(_footOffset);
        var ray = new Ray(rayOrigin, Vector3.down);
        Debug.DrawLine(body.position, rayOrigin, Color.green);

        if (_lerp < 1) return;
        if(!Physics.Raycast(ray, out var info, 10, _terrainLayer.value)) return;
        var distance = Vector3.Distance(NewPosition, info.point);
        if (distance > _stepDistance)
        {
            _lerp = 0;
            var direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(NewPosition).z ? 1 : -1;
            NewPosition = info.point + body.forward * (direction * _stepLength);
            NewNormal = info.normal;
        }
    }

    public void ForceRepositionTarget()
    {
        var body = _spider.transform;
        var rayOrigin = Vector3.up / 2 + body.position + body.TransformDirection(_footOffset);
        var ray = new Ray(rayOrigin, Vector3.down);
        
        if (_lerp < 1) return;
        if(!Physics.Raycast(ray, out var info, 10, _terrainLayer.value)) return;
        var distance = Vector3.Distance(NewPosition, info.point);
        if (distance > _stepDistance / 4)
        {
            _lerp = 0;
            NewPosition = info.point;
            NewNormal = info.normal;
        }
    }

    public bool IsMoving() => _lerp < 1;

    [ContextMenu("Re-assign foot offset")]
    public void ReAssignFootOffset()
    {
        _footOffset = transform.localPosition;
        _footOffset.x = MathF.Round(_footOffset.x, 2);
        _footOffset.y = MathF.Round(_footOffset.y, 2);
        _footOffset.z = MathF.Round(_footOffset.z, 2);
    }
}
