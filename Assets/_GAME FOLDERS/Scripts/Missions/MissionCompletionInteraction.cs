using UnityEngine;

public class MissionCompletionInteraction : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private MissionType missionType;
    [SerializeField] private string targetMissionName;

    public void OnMissionCompleted()
    {
        missionManager.OnTriggerMissionCompleted(missionType, targetMissionName);
    }
}
