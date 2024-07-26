using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Missions")]
    [SerializeField] private List<Mission> missions = new List<Mission>();

    [Header("Mission Panel Controller")]
    private MissionPanelController missionPanelController;

    [Header("UI Controller")]
    [SerializeField] private CanvasGroup timerTextParent;
    [SerializeField] private TextMeshProUGUI timerText;


    [Header("Timer Settings")]
    [SerializeField] private float timerStartDelay = 10f; // Timer'in görev popup'ý çýktýktan ne kadar sonra baþlayacaðý
    [SerializeField] private float missionStartDelay = 45f; // Ýlk görevin baþlamasý için belirlenen süre
    [SerializeField] private Vector2 randomMissionStartRange = new Vector2(10f, 20f); // Rastgele süre aralýðý
    [SerializeField] private float nextMissionDelay = 30f; // Bir görev tamamlandýktan veya baþarýsýz olduktan sonra bekleme süresi

    [Header("Settings")]
    [SerializeField] private bool useRandomStartTime = false; // Rastgele süre kullanýmý


    private int currentMissionIndex = 0;
    private Coroutine missionTimerCoroutine;

    private void Awake()
    {
        missionPanelController = GetComponent<MissionPanelController>();
    }
    private void Start()
    {
        timerTextParent.alpha = 0f;

        if (missions.Count > 0)
        {
            float initialDelay = useRandomStartTime ? Random.Range(randomMissionStartRange.x, randomMissionStartRange.y) : missionStartDelay;
            StartCoroutine(StartMissionAfterDelay(initialDelay));
        }
    }
    private IEnumerator StartMissionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextMission();
    }
    private void StartNextMission()
    {
        if (currentMissionIndex < missions.Count)
        {
            var missionObject = missions[currentMissionIndex].MissionObject;
            if (missionObject != null)
            {
                missionObject.GetComponent<Collider>().enabled = true;
                missionObject.GetComponent<MissionInteraction>().enabled = true;
            }
            missionPanelController.SetMissionText(missions[currentMissionIndex].Description);
            missionPanelController.ShowMissionPanel();
            missionTimerCoroutine = StartCoroutine(MissionTimer(missions[currentMissionIndex].Duration));
        }
        else
        {
            Debug.Log("All missions completed!");
            missionPanelController.HideTabInfo();
            // Tüm görevler tamamlandýðýnda yapýlacak iþlemler
        }
    }
    public void CompleteCurrentMission()
    {
        missionPanelController.HideTabInfo();
        Debug.Log($"Mission: '{missions[currentMissionIndex].Description}' completed!");
        timerTextParent.DOFade(0, 0.15f);
        if (missionTimerCoroutine != null)
        {
            StopCoroutine(missionTimerCoroutine);
        }
        // Gorev tamamlandýðýnda diger gorev icin sirali kontrolu
        var missionObject = missions[currentMissionIndex].MissionObject;
        if (missionObject != null)
        {
            missionObject.GetComponent<Collider>().enabled = false;
            missionObject.GetComponent<MissionInteraction>().enabled = false;
        }

        missionPanelController.ShowMissionCompletePopup();
        currentMissionIndex++;
        StartCoroutine(StartMissionAfterDelay(nextMissionDelay));
    }
    public void OnTriggerMissionCompleted(string missionName)
    {
        if (missions[currentMissionIndex].Description.Contains(missionName))
        {
            CompleteCurrentMission();
        }
    }
    public void OnTriggerMissionCompleted(MissionType missionType, string targetName)
    {
        if (missions[currentMissionIndex].Type == missionType && missions[currentMissionIndex].TargetName == targetName)
        {
            CompleteCurrentMission();
        }
    }

    private IEnumerator MissionTimer(float duration)
    {
        timerTextParent.DOFade(0, 0.15f);
        yield return new WaitForSeconds(timerStartDelay);
        timerTextParent.DOFade(1, 0.25f);

        float remainingTime = duration;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI(remainingTime);
            yield return null;
        }

        // Görev süresi dolduðunda yapýlacak iþlemler
        missionPanelController.ShowGameOver();
        Debug.Log("Mission failed: " + missions[currentMissionIndex].Description);
    }
    private void UpdateTimerUI(float remainingTime)
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}

[System.Serializable]
public class Mission
{
    public string Description;
    public float Duration;
    public MissionType Type;
    public string TargetName;
    public GameObject MissionObject;
}

public enum MissionType
{
    ZombieKill, // Zombi öldürme ve inceleme görevlerinde kullanýlacak
    ObjectInspection, // Nesne bulup inceleme
    CodeBreaking, // Þifre çözme iþlemlerinde 
    LocationExploration // Belirli yerleri incelem görevleri
}
