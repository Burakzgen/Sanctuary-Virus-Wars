using UnityEngine;

public class MissionInteraction : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private MissionType missionType;
    [SerializeField] private string targetMissionName;

    private void Start()
    {
        if (missionManager == null)
        {
            missionManager = FindObjectOfType<MissionManager>();
        }
        this.enabled = false;
    }
    public void OnMissionCompleted()
    {
        missionManager.OnTriggerMissionCompleted(missionType, targetMissionName);
    }
}
