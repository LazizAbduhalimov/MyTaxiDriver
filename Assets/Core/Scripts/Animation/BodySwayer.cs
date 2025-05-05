using UnityEngine;

public class BodySwayer : MonoBehaviour
{
    public float swayAmount = 2f;
    public float swaySpeed = 4f;
    public Transform spiderBody;

    private Vector3 initialRotation;

    void Start()
    {
        if (spiderBody == null) spiderBody = transform;
        initialRotation = spiderBody.localEulerAngles;
    }

    void Update()
    {
            // Расчёт раскачивания по синусоиде
            var swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            var swayZ = Mathf.Sin(Time.time * swaySpeed * 0.5f) * swayAmount * 0.5f;

            // Применение покачивания
            spiderBody.localEulerAngles = initialRotation + new Vector3(swayX, 0f, swayZ);
            // spiderBody.localEulerAngles = Vector3.Lerp(spiderBody.localEulerAngles, initialRotation, Time.deltaTime * 5f);
    }
}