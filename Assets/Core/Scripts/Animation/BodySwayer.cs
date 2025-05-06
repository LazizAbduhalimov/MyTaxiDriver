using UnityEngine;

public class BodySwayer : MonoBehaviour
{
    public float swayAmount = 2f;
    public float swaySpeed = 4f;
    public Transform spiderBody;

    private Vector3 initialRotation;
    private Vector3 LastSway;

    void Start()
    {
        if (spiderBody == null) spiderBody = transform;
    }

    void Update()
    {
        // Расчёт раскачивания по синусоиде
        var swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        var swayZ = Mathf.Sin(Time.time * swaySpeed * 0.5f) * swayAmount * 0.5f;
        var initialRotation = spiderBody.localEulerAngles - LastSway;
        LastSway = new Vector3(swayX, 0f, swayZ);
        var swayRotation = initialRotation + LastSway;

        // Применяем качку к текущей ориентации, используя компонент вращения
        spiderBody.localRotation = Quaternion.Euler(swayRotation);
    }
}