using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TargetMover))]
public class TargetMoverGizmos : MonoBehaviour
{
    public TargetMover _targetMover;

    private void OnValidate()
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
        if (Application.isPlaying || _targetMover == null) return;
        var spider = _targetMover.Spider;
        var position = spider.transform.position + _targetMover.FootOffset;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(position.AddZ(-_targetMover.StepLength), position.AddZ(_targetMover.StepLength));
        Gizmos.DrawLine(position.AddX(-_targetMover.StepLength), position.AddX(_targetMover.StepLength));
        Gizmos.DrawSphere(position, 0.1f);
        ExtendedGizmos.DrawCircle(position, _targetMover.StepDistance);
        ExtendedGizmos.DrawEllipse(position, _targetMover.StepDistance * 2, _targetMover.StepHeight * 2, plane: ExtendedGizmos.Plane.XY);
    }
}