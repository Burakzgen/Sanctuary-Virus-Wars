using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Button[] showButtons;
    [SerializeField] private Button[] hideButtons;

    private GameObject _currentPanel;

    private void Start()
    {
        ShowPanel(0);

        for (int i = 0; i < showButtons.Length; i++)
        {
            int index = i;
            showButtons[index].onClick.AddListener(() => ShowPanel(index));
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
