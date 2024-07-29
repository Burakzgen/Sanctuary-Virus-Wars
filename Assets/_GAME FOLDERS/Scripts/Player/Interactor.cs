using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField][Range(0, 10)] private float _raycastRange = 1f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject crossImage;
    [SerializeField] private GameObject interactETextImage;
    PlayerHealth _playerHealth;
    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        crossImage.gameObject.SetActive(true);
        interactETextImage.gameObject.SetActive(false);
    }
    private void Update()
    {
        CheckForInteractable();
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformInteraction();
        }
    }
    private void CheckForInteractable()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float effectiveRange = _raycastRange;


        if (Physics.Raycast(ray, out RaycastHit hitInfo, effectiveRange, _layerMask))
        {
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                crossImage.gameObject.SetActive(false);
                interactETextImage.gameObject.SetActive(true);
                return;
            }
        }

        interactETextImage.gameObject.SetActive(false);
        crossImage.gameObject.SetActive(true);
    }
    private void PerformInteraction()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float effectiveRange = _raycastRange;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, effectiveRange, _layerMask))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(_playerHealth);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[3]);
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

        Gizmos.DrawRay(ray.origin, ray.direction * effectiveRange);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, effectiveRange, _layerMask))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hitInfo.point, 0.1f);
        }
    }
}
