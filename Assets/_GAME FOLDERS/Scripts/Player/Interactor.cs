using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField][Range(0, 10)] private float _raycastRange = 1f;
    [SerializeField] private LayerMask _layerMask;
    private Camera _cam;
    private CameraController _cameraController;

    private void Start()
    {
        _cam = Camera.main;
        _cameraController = _cam.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformInteraction();
        }
    }

    private void PerformInteraction()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float effectiveRange = _raycastRange;

        if (_cameraController != null && _cameraController.cameraMode == CameraController.CameraMode.ThirdPerson)
        {
            effectiveRange = Mathf.Max(_raycastRange, _raycastRange + _cameraController.thirdPersonSettings.distance);
        }

        if (Physics.Raycast(ray, out RaycastHit hitInfo, effectiveRange, _layerMask))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
            if (_cam == null) return;
        }

        Gizmos.color = Color.red;
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float effectiveRange = _raycastRange;

        if (_cameraController != null && _cameraController.cameraMode == CameraController.CameraMode.ThirdPerson)
        {
            // Adjust range for third person if necessary
            effectiveRange = Mathf.Max(_raycastRange, _raycastRange + _cameraController.thirdPersonSettings.distance);
        }

        Gizmos.DrawRay(ray.origin, ray.direction * effectiveRange);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, effectiveRange, _layerMask))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hitInfo.point, 0.1f);
        }
    }
}
