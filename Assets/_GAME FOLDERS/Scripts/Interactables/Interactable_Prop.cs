using TMPro;
using UnityEngine;

public class Interactable_Prop : MonoBehaviour, IInteractable
{
    public enum MODELTYPE
    {
        UI,
        INTERACTIVE_MODEL
    }
    public enum ROTATETYPE
    {
        X,
        Y,
        Z,
        ALL
    }
    [SerializeField] MODELTYPE _modelType = MODELTYPE.INTERACTIVE_MODEL;
    [SerializeField] ROTATETYPE _rotateType = ROTATETYPE.ALL;

    #region GENERAL_CONTROLS

    [Header("*** GENERAL CONTROLS ***")]
    // Public
    [Header("Referances")]
    FirstPersonMovement m_CharacterMovement;
    FirstPersonLook m_FirstPersonLook;
    // Private
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startScale;
    bool _isPickUp = false; // Tüm objelerinin alýnýp alýnmadýðýnýn kontrolu.
    bool _isModelObject = false; // Sadece model üzerinde döndürme olacaðý için kontrol amaçlý oluþturdu.
    Collider _objectCollider;
    #endregion

    #region UI TYPE CONTROLS

    [Header("*** UI TYPE CONTROLS ***")]
    [Header("Referances")]
    [SerializeField][TextArea] private string _notes;
    private GameObject _canvasUIInfoPanel;
    private TextMeshProUGUI _UIInfoTexts;

    // Read mode Controller
    // [Header("Read Mode Controls")]
    bool _isReadModeActive = false;
    bool _isReadModeUse = false;
    private GameObject _blurredEffectImage;
    private TextMeshProUGUI _readModeText;

    //[Header("Image Controls")]
    //[SerializeField] private Image _spriteImage; // TODO: Duruma göre ilave edilecek. 


    #endregion

    #region INTERACTIVE MODEL TYPE CONTROLS
    [Header("*** INTERACTIVE_MODEL TYPE CONTROLS ***")]

    [Header("Transform")]
    public Transform _setTransformPos;
    public Transform _setTransformParent;
    [SerializeField] Vector3 _setPositionOffset;
    [SerializeField] Quaternion _setRotationOffset;
    [SerializeField] float _scale = 1;

    [Header("UI")]
    private GameObject _canvasModelInfoPanel;

    [Header("Paper Settings")]
    [SerializeField] private float rotateSpeed = 250f;
    #endregion

    #region UI CONTROLLER METHODS
    private void SetText(string notes)
    {
        if (_isPickUp)
            return;

        _UIInfoTexts.text = notes;
        _readModeText.text = notes;
        _canvasUIInfoPanel.SetActive(true);
        m_CharacterMovement.IsPause = true;
        m_FirstPersonLook.enabled = false;

        _isReadModeUse = true;
        _isPickUp = true;
    }
    private void CloseNote()
    {
        _canvasUIInfoPanel.SetActive(false);
        _isPickUp = false;
        m_CharacterMovement.IsPause = false;
        m_FirstPersonLook.enabled = true;
        ReadTextMode();
    }
    private void ReadTextMode()
    {
        if (!_isReadModeUse)
            return;

        _isReadModeActive = !_isReadModeActive;
        _blurredEffectImage.SetActive(_isReadModeActive);
    }
    private string GetNote()
    {
        return _notes;
    }
    private void UIInteract()
    {
        if (_isPickUp && _isReadModeUse)
            ReadTextMode();

        SetText(GetNote());

    }
    private void UI_Drop()
    {
        CloseNote();
    }
    #endregion

    #region INTERACTIVE MODEL CONTROLLER METHODS

    private void InteractiveModel_Interact()
    {
        if (_isPickUp)
            return;

        _isPickUp = true;
        // Transform noktalarýnýn ayarlanmasý.
        //transform.SetPositionAndRotation(_setTransformPos.position, _setTransformPos.rotation);

        transform.SetPositionAndRotation(_setTransformPos.position + _setPositionOffset, _setTransformPos.rotation * _setRotationOffset);
        transform.parent = _setTransformPos;
        transform.localScale = _startScale * _scale;

        // UI objelerinin aktif edilmesi.
        _canvasModelInfoPanel.SetActive(true);
        _objectCollider.enabled = false;
        // Karakter kontrol deðiþiklikleri
        m_CharacterMovement.IsPause = true;
        m_FirstPersonLook.enabled = false;
        _isModelObject = true;
    }
    private void InteractiveModel_Rotate()
    {
        //float rotateX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        //float rotateY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        //this.transform.Rotate(Camera.main.transform.up, -rotateX, Space.World);
        //this.transform.Rotate(Camera.main.transform.right, rotateY, Space.World);

        float rotateX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float rotateY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        switch (_rotateType)
        {
            case ROTATETYPE.X:
                this.transform.Rotate(Vector3.right, rotateY, Space.World);
                break;
            case ROTATETYPE.Y:
                this.transform.Rotate(Vector3.up, -rotateX, Space.World);
                break;
            case ROTATETYPE.Z:
                this.transform.Rotate(Vector3.forward, rotateY, Space.World);
                break;
            case ROTATETYPE.ALL:
                this.transform.Rotate(Camera.main.transform.up, -rotateX, Space.World);
                this.transform.Rotate(Camera.main.transform.right, rotateY, Space.World);
                break;
        }

    }

    private void InteractiveModel_Drop()
    {
        // Transform defaul deðerlerinin alýnmasý ve deðiþtirilmesi.
        if (!_setTransformParent)
            transform.parent = _setTransformParent;
        else
            transform.parent = null;

        transform.SetPositionAndRotation(_startPosition, _startRotation);
        transform.localScale = _startScale;

        // UI kontrollerinin devre dýþý býrakýlmasý
        _canvasModelInfoPanel.SetActive(false);
        _objectCollider.enabled = true;

        // Karakter kontrollerinin devre dýþý býrakýlmasý.
        m_CharacterMovement.IsPause = false;
        m_FirstPersonLook.enabled = true;

        _isPickUp = false;
        _isModelObject = false;
    }
    #endregion

    #region MONOBEHAVIOUR METHODS
    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        m_FirstPersonLook = player.GetComponentInChildren<FirstPersonLook>();
        m_CharacterMovement = player.GetComponent<FirstPersonMovement>();

        if (gameObject.layer != 6)
            gameObject.layer = 6;
    }
    private void Start()
    {
        SetReferance();
        _objectCollider = GetComponent<Collider>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startScale = transform.localScale;
    }
    void SetReferance()
    {
        // UI TYPE
        _canvasUIInfoPanel = UIReferanceManager.Instance.m_UIInteractionPanel;
        _UIInfoTexts = _canvasUIInfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _blurredEffectImage = _canvasUIInfoPanel.transform.GetChild(2).gameObject;
        _readModeText = _blurredEffectImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        // MODEL TYPE
        _canvasModelInfoPanel = UIReferanceManager.Instance.m_ModelInteractionPanel;
    }
    private void Update()
    {
        if (!_isPickUp)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropInteract();
        }

        if (Input.GetMouseButton(1) && _isModelObject)
        {
            InteractiveModel_Rotate();
        }
    }
    public void Interact(PlayerHealth health)
    {
        switch (_modelType)
        {
            case MODELTYPE.UI:
                UIInteract();
                break;
            case MODELTYPE.INTERACTIVE_MODEL:
                InteractiveModel_Interact();
                break;
        }
    }
    private void DropInteract()
    {
        switch (_modelType)
        {
            case MODELTYPE.UI:
                UI_Drop();
                break;
            case MODELTYPE.INTERACTIVE_MODEL:
                InteractiveModel_Drop();
                break;
        }
    }
    #endregion
}
