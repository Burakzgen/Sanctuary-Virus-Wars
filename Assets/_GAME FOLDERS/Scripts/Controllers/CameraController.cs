using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        ThirdPerson,
        FirstPerson
    }

    [SerializeField] Transform followTarget;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5f;
    [SerializeField] float minVerticalAngle = -45f;
    [SerializeField] float maxVerticalAngle = 45f;
    [SerializeField] Vector2 framingOffset;
    [SerializeField] bool invertCameraX;
    [SerializeField] bool invertCameraY;
    public CameraMode cameraMode = CameraMode.ThirdPerson;
    [SerializeField] Vector3 firstPersonOffset = new Vector3(0f, 0.6f, 0f);

    float rotationY;
    float rotationX;
    float invertXVal;
    float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invertXVal = (invertCameraX) ? -1 : 1;
        invertYVal = (invertCameraY) ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        if (cameraMode == CameraMode.ThirdPerson)
        {
            var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
            transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
            transform.rotation = targetRotation;
        }
        else
        {
            transform.position = followTarget.position + followTarget.TransformDirection(firstPersonOffset);
            transform.rotation = targetRotation;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleCameraMode();
        }
    }

    void ToggleCameraMode()
    {
        if (cameraMode == CameraMode.ThirdPerson)
        {
            cameraMode = CameraMode.FirstPerson;
        }
        else
        {
            cameraMode = CameraMode.ThirdPerson;
        }
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}