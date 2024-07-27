using UnityEngine;
using UnityEngine.Events;

public class MissionTriggerObjects : MonoBehaviour
{
    public UnityEvent OnMissionCompleted;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnMissionCompleted?.Invoke();
        }
    }
}
