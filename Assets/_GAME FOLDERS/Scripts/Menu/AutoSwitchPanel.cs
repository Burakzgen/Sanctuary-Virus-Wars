using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoSwitchPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject[] buttonIcons;
    [SerializeField] private Button[] showButtons;
    [SerializeField] private float switchInterval = 10f;

    private GameObject _currentPanel;
    private GameObject _currentIcon;
    private int _currentIndex = 0;
    private bool _autoSwitchActive = true;
    private Coroutine _autoSwitchCoroutine;

    private void Start()
    {
        ShowPanel(0);

        for (int i = 0; i < showButtons.Length; i++)
        {
            int index = i;
            showButtons[index].onClick.AddListener(() =>
            {
                if (_autoSwitchCoroutine != null)
                {
                    StopCoroutine(_autoSwitchCoroutine); // Elle týklanýrsa otomatik geçiþi durdur
                }
                ShowPanel(index);
                if (_autoSwitchActive)
                {
                    _autoSwitchCoroutine = StartCoroutine(AutoSwitchPanelsCoroutine()); // Elle týklamadan sonra otomatik geçiþi tekrar baþlat
                }
            });
        }

        // Baþlangýçta otomatik geçiþi baþlat
        _autoSwitchCoroutine = StartCoroutine(AutoSwitchPanelsCoroutine());
    }

    public void ShowPanel(int index)
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
        _currentIndex = index;

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

    public void CloseCurrentPanel()
    {
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
            _currentPanel = null;
        }
    }

    private IEnumerator AutoSwitchPanelsCoroutine()
    {
        while (_autoSwitchActive)
        {
            yield return new WaitForSeconds(switchInterval);
            AutoSwitchPanelController();
        }
    }

    private void AutoSwitchPanelController()
    {
        _currentIndex = (_currentIndex + 1) % panels.Length;
        ShowPanel(_currentIndex);
    }
}
