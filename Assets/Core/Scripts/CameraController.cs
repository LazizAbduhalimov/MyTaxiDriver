using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float moveSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        HandleZoom();
        HandleMovement();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0, moveZ);
    }
}