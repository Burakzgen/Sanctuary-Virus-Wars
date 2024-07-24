using UnityEngine;

public class ZombieKill : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private string targetMissionName;
    private void Start()
    {
        if (missionManager == null)
        {
            missionManager = FindObjectOfType<MissionManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnZombieKilled();
            Destroy(gameObject);
        }
    }
    public void OnZombieKilled()
    {
        missionManager.OnTriggerMissionCompleted(MissionType.ZombieKill, targetMissionName);
    }
}
