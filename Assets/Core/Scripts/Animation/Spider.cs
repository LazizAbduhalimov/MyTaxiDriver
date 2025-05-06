using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public List<TargetMover> groupA;
    public List<TargetMover> groupB;
    public float stepInterval = 0.3f; // пауза между группами

    private float timer;
    private bool isGroupATurn = true;

    void Update()
    {
        timer += Time.deltaTime;

        // Если все ноги текущей группы уже не двигаются и интервал прошёл
        // var group = isGroupATurn ? groupA : groupB;
        // Interanl(group);
        // if (!IsGroupMoving(group))
        // {
        //     // Принудительно обнови позицию ног в этой группе (триггер начала шага)
        //     TriggerStep(group);
        //
        //     // Переключи группу
        //     isGroupATurn = !isGroupATurn;
        //     timer = 0f;
        // }
    }

    bool IsGroupMoving(List<TargetMover> group)
    {
        return group.Any(leg => leg.IsMoving());
    }

    void TriggerStep(List<TargetMover> group)
    {
        Debug.Log("Reposition");
        foreach (var leg in group)
        {
            leg.RepositionTarget();
        }
    }

    private void Interanl(List<TargetMover> group)
    {
        foreach (var leg in group)
        {
            // leg.Internal();
        }
    }
}
