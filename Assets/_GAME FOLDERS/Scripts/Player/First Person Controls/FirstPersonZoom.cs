using UnityEngine;

public class FirstPersonZoom : MonoBehaviour
{
    Camera _cam;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 1;
    void Awake()
    {
        _cam = GetComponent<Camera>();
        if (_cam)
        {
            defaultFOV = _cam.fieldOfView;
        }
    }
    void Update()
    {
        currentZoom += Input.mouseScrollDelta.y * sensitivity * .05f;
        currentZoom = Mathf.Clamp01(currentZoom);
        _cam.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
    }
}
