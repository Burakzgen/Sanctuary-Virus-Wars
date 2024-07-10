using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 15, player.transform.position.z);
    }
}
