using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engaging : MonoBehaviour
{
    public Transform Victim;
    public float Speed = 3f;
    public float RotationSpeed = 5f;

    void Update()
    {
        if (Victim == null) return;
        if (Vector3.Distance(Victim.position, transform.position) < 2) return;
        
        // Поворачиваемся к цели по всем осям
        var direction = (transform.position - Victim.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Victim.rotation = Quaternion.Slerp(Victim.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        // Двигаемся вперёд (вдоль forward)
        Victim.position += Victim.forward * Speed * Time.deltaTime;
    }
}
