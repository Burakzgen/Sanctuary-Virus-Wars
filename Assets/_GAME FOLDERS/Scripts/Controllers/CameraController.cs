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
    [SerializeField] Vector2 framingOffset;
    [SerializeField] bool invertCameraX;
    [SerializeField] bool invertCameraY;

    [SerializeField] public CameraSettingsData thirdPersonSettings;
    [SerializeField] CameraSettingsData firstPersonSettings;

    public CameraMode cameraMode = CameraMode.ThirdPerson;

    float rotationY;
    float rotationX;
    float invertXVal;
    float invertYVal;
    private Camera cam;
    private CameraSettingsData currentSettings;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
        UpdateCameraSettings();
    }

    private void Update()
    {
        HandleRotation();
        UpdateCameraPosition();
        HandleCameraModeToggle();
    }

    void HandleRotation()
    {
        invertXVal = (invertCameraX) ? -1 : 1;
        invertYVal = (invertCameraY) ? -1 : 1;
        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, currentSettings.minVerticalAngle, currentSettings.maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
    }

    void UpdateCameraPosition()
    {
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        if (cameraMode == CameraMode.ThirdPerson)
        {
            var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
            transform.position = focusPosition - targetRotation * new Vector3(0, 0, currentSettings.distance);
        }
        else
        {
            transform.position = followTarget.position + followTarget.TransformDirection(currentSettings.cameraOffset);
        }

        transform.rotation = targetRotation;
        cam.fieldOfView = currentSettings.fieldOfView;
    }

    void HandleCameraModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleCameraMode();
        }
    }

    void ToggleCameraMode()
    {
        cameraMode = (cameraMode == CameraMode.ThirdPerson) ? CameraMode.FirstPerson : CameraMode.ThirdPerson;
        UpdateCameraSettings();
    }

    void UpdateCameraSettings()
    {
        currentSettings = (cameraMode == CameraMode.ThirdPerson) ? thirdPersonSettings : firstPersonSettings;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}