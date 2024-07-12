using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject[] buttonIcons;
    [SerializeField] private Button[] showButtons;
    [SerializeField] private Button[] hideButtons;

    private GameObject _currentPanel;
    private GameObject _currentIcon;

    private void Start()
    {
        ShowPanel(0);

        for (int i = 0; i < showButtons.Length; i++)
        {
            int index = i;
            showButtons[index].onClick.AddListener(() => ShowPanel(index));
            if (hideButtons.Length > 0)
                hideButtons[index].onClick.AddListener(CloseCurrentPanel);
        }
    }

    private void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length)
        {
            Debug.LogError("Invalid panel index");
            return;
        }

        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
        }

        _currentPanel = panels[index];
        _currentPanel.SetActive(true);

        if (buttonIcons.Length > 0)
        {
            if (_currentIcon != null)
            {
                _currentIcon.SetActive(false);
            }

            _currentIcon = buttonIcons[index];
            _currentIcon.SetActive(true);
        }
    }

    private void CloseCurrentPanel()
    {
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
            _currentPanel = null;
        }
    }
}
