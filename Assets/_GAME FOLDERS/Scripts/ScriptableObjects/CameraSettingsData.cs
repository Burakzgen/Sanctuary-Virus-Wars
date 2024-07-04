using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Settings", menuName = "Camera/Camera Settings", order = 0)]
public class CameraSettingsData : ScriptableObject
{
    public float fieldOfView = 60f;
    public float minVerticalAngle = -45f;
    public float maxVerticalAngle = 45f;
    public Vector3 cameraOffset = Vector3.zero;
    public float distance = 5f; // Third person için gerekli
}