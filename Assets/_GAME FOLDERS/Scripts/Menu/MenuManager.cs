using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Sprite newGameSprite;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Sprite quitSprite;
    [SerializeField] private PopupManager popupManager;

    private void Start()
    {
        newGameButton.onClick.AddListener(OnNewGameClicked);
        quitGameButton.onClick.AddListener(OnQuitGameClicked);
    }

    private void OnNewGameClicked()
    {
        popupManager.ShowPopup("Are you sure you want to start a new game?", "NEW GAME", newGameSprite, ConfirmNewGame);
    }

    private void OnQuitGameClicked()
    {
        popupManager.ShowPopup("Are you sure you want to quit the game?", "EXIT", quitSprite, ConfirmQuitGame);
    }

    private void ConfirmNewGame()
    {
        Debug.Log("New Game Started!");
    }

    private void ConfirmQuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
