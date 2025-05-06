using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    private Spider Spider;
    private Vector3 lastPosition;
    private float lastChangeTime;
    public float stationaryThreshold = .5f; // сколько секунд объект должен быть неподвижен
    private bool enabled = true;

    void Start()
    {
        Spider = GetComponent<Spider>();
        lastPosition = transform.position;
        lastChangeTime = Time.time;
    }

    void Update()
    {
        var currentPosition = transform.position;

        if (currentPosition != lastPosition)
        {
            lastPosition = currentPosition;
            lastChangeTime = Time.time;
            enabled = true;
        }
        else
        {
            if (Time.time - lastChangeTime > stationaryThreshold && enabled)
            {
                Debug.Log("Объект не двигается!");
                // foreach (var leg in Spider.groupA)
                // {
                //     leg.ForceRepositionTarget();
                // }                
                // foreach (var leg in Spider.groupB)
                // {
                //     leg.ForceRepositionTarget();
                // }

                enabled = false;
            }
        }
    }
}