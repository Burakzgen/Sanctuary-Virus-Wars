using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] MenuManager m_MenuManager;
    [SerializeField] Button submitButton;
    [SerializeField] Button getLeaderboardButton;

    [SerializeField] Transform rowsParent;
    [SerializeField] GameObject rowPrefab;

    private const string NICKNAME_KEY = "Nickname";
    private string HIGHEST_SCORE_KEY = "HighestZombieKillCount";

    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt(HIGHEST_SCORE_KEY));
        submitButton.onClick.AddListener(SubmitNameButton);
        getLeaderboardButton.onClick.AddListener(GetLeaderboard);
        Login();
    }
    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserData = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }
    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful account login/creation!");

        string playfabName = null;
        string localName = PlayerPrefs.GetString(NICKNAME_KEY, "");
        int localScore = PlayerPrefs.GetInt(HIGHEST_SCORE_KEY, 0);

        if (result.InfoResultPayload != null)
        {
            if (result.InfoResultPayload.PlayerProfile != null)
            {
                playfabName = result.InfoResultPayload.PlayerProfile.DisplayName;
            }

            if (result.InfoResultPayload.UserData != null &&
                result.InfoResultPayload.UserData.ContainsKey(HIGHEST_SCORE_KEY))
            {
                int playfabScore = int.Parse(result.InfoResultPayload.UserData[HIGHEST_SCORE_KEY].Value);
                SyncScore(localScore, playfabScore);
            }
            else
            {
                UpdatePlayFabScore(localScore);
            }
            UpdateLeaderboard();

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                SyncNickname(localName, playfabName);
            }

            Debug.Log("Login and data sync complete");

        }
    }
    private void SyncNickname(string localName, string playfabName)
    {
        if (!string.IsNullOrEmpty(playfabName))
        {
            PlayerPrefs.SetString(NICKNAME_KEY, playfabName);
            m_MenuManager.nickNameText.text = playfabName;
        }
        else if (!string.IsNullOrEmpty(localName))
        {
            UpdatePlayFabName(localName);
            m_MenuManager.nickNameText.text = localName;
        }
        else
        {
            m_MenuManager.inputPanel.SetActive(true);
        }
    }

    private void SyncScore(int localScore, int playfabScore)
    {
        int highestScore = Mathf.Max(localScore, playfabScore);
        PlayerPrefs.SetInt(HIGHEST_SCORE_KEY, highestScore);

        if (highestScore != playfabScore)
        {
            UpdatePlayFabScore(highestScore);
        }
    }
    private void UpdatePlayFabName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    private void UpdatePlayFabScore(int score)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {HIGHEST_SCORE_KEY, score.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnUserDataUpdate, OnError);
    }
    private void OnUserDataUpdate(UpdateUserDataResult result)
    {
        Debug.Log("Updated user data on PlayFab!");
    }
    void UpdateLeaderboard()
    {
        int score = PlayerPrefs.GetInt("HighestZombieKillCount", 0);
        SendLeaderboard(score);
    }
    void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = m_MenuManager.nicknameInputField.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        UpdateLeaderboard();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging creating account!" + error.GenerateErrorReport());
    }
    void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Test",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }
    void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Test",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        UpdateLeaderboardUI(result.Leaderboard);

        /*
         *       foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGO = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newGO.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }
         */
    }
    private void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            GameObject row;
            if (i < rowsParent.childCount)
            {
                row = rowsParent.GetChild(i).gameObject;
            }
            else
            {
                row = Instantiate(rowPrefab, rowsParent);
            }

            var item = leaderboard[i];
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }

        while (rowsParent.childCount > leaderboard.Count)
        {
            DestroyImmediate(rowsParent.GetChild(rowsParent.childCount - 1).gameObject);
        }
    }
    // Oyun içi çaðýrmak için oluþturuldu.Þuanlýk ihtiyaç yok. 
    public void SetNewNickname(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            PlayerPrefs.SetString(NICKNAME_KEY, newName);
            m_MenuManager.nickNameText.text = newName;
            UpdatePlayFabName(newName);
        }
    }
    public void UpdateScore(int newScore)
    {
        int currentHighScore = PlayerPrefs.GetInt(HIGHEST_SCORE_KEY, 0);
        if (newScore > currentHighScore)
        {
            PlayerPrefs.SetInt(HIGHEST_SCORE_KEY, newScore);
            UpdatePlayFabScore(newScore);
            SendLeaderboard(newScore);
        }
    }
}
