using UnityEngine;

public class FirstPersonZoom : MonoBehaviour
{
    Camera _cam;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 1;
    FirstPersonMovement m_FirstPersonMovement;

    void Awake()
    {
        m_FirstPersonMovement = transform.GetComponentInParent<FirstPersonMovement>();
        _cam = GetComponent<Camera>();
        if (_cam)
        {
            defaultFOV = _cam.fieldOfView;
        }
    }
    void Update()
    {
        if (m_FirstPersonMovement.IsPause) return;

        currentZoom += Input.mouseScrollDelta.y * sensitivity * .05f;
        currentZoom = Mathf.Clamp01(currentZoom);
        _cam.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
    }
}
