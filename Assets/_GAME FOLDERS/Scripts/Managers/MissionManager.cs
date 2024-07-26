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
    [SerializeField] private float timerStartDelay = 10f; // Timer'in g�rev popup'� ��kt�ktan ne kadar sonra ba�layaca��
    [SerializeField] private float missionStartDelay = 45f; // �lk g�revin ba�lamas� i�in belirlenen s�re
    [SerializeField] private Vector2 randomMissionStartRange = new Vector2(10f, 20f); // Rastgele s�re aral���
    [SerializeField] private float nextMissionDelay = 30f; // Bir g�rev tamamland�ktan veya ba�ar�s�z olduktan sonra bekleme s�resi

    [Header("Settings")]
    [SerializeField] private bool useRandomStartTime = false; // Rastgele s�re kullan�m�


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
            // T�m g�revler tamamland���nda yap�lacak i�lemler
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
        // Gorev tamamland���nda diger gorev icin sirali kontrolu
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

        // G�rev s�resi doldu�unda yap�lacak i�lemler
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
    ZombieKill, // Zombi �ld�rme ve inceleme g�revlerinde kullan�lacak
    ObjectInspection, // Nesne bulup inceleme
    CodeBreaking, // �ifre ��zme i�lemlerinde 
    LocationExploration // Belirli yerleri incelem g�revleri
}
