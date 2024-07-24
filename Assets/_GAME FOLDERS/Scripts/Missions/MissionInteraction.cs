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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnMissionCompleted();
            if (missionType == MissionType.ZombieKill)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnMissionCompleted()
    {
        missionManager.OnTriggerMissionCompleted(missionType, targetMissionName);
    }
}
