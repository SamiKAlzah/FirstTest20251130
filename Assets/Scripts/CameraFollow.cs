using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 8f;
    public float mouseSensitivity = 2f;

    float rotX;
    float rotY;

    void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotY = Mathf.Clamp(rotY, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position);
    }
}
