using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engaging : MonoBehaviour
{
    public Transform Victim;
    public Spider Spider;
    public float Speed = 3f;
    public float RotationSpeed = 5f;

    void Update()
    {
        if (Victim == null) return;
        Spider.IsWalking = false;
        if (Vector3.Distance(Victim.position, transform.position) < 2) return;
        Spider.IsWalking = true;
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
