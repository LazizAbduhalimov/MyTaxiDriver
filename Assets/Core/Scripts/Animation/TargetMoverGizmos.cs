using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TargetMover))]
public class TargetMoverGizmos : MonoBehaviour
{
    private TargetMover _targetMover;

    private void Awake()
    {
        _targetMover = GetComponent<TargetMover>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || _targetMover == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetMover.NewPosition, 0.25f);
    }

    private void OnDrawGizmosSelected()
    {
        if (_targetMover == null || _targetMover.IsMoving()) return;
        var spider = _targetMover.Spider;
        if (spider == null) return;

        var position = spider.transform.position + _targetMover.FootOffset;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(position, 0.1f);
        ExtendedGizmos.DrawCircle(position, _targetMover.StepDistance);
        ExtendedGizmos.DrawEllipse(position, _targetMover.StepDistance * 2, _targetMover.StepHeight * 2, plane: ExtendedGizmos.Plane.XY);
    }
}