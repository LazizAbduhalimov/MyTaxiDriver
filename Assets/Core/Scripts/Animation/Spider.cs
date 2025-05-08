using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public List<TargetMover> groupA;
    public List<TargetMover> groupB;

    private bool _isGroupAMoving = true;
    public bool IsWalking;

    private void Update()
    {
        if (!IsWalking) {
            if (IsAnyLegMoving()) return;
            var activeGroup = _isGroupAMoving ? groupB : groupA;
            foreach (var leg in activeGroup)
            {
                leg.ForceRepositionTarget();
            }
            _isGroupAMoving = !_isGroupAMoving;
            return;
        }
        
        if (!IsAnyLegMoving())
        {
            var activeGroup = _isGroupAMoving ? groupB : groupA;
            foreach (var leg in activeGroup)
            {
                leg.RepositionTarget();
            }
            _isGroupAMoving = !_isGroupAMoving;
        }
    }

    private bool IsAnyLegMoving()
    {
        return groupA.Any(leg => leg.IsMoving()) || groupB.Any(leg => leg.IsMoving());
    }
}
