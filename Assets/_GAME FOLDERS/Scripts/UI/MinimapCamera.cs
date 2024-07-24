using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    private void LateUpdate()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.y = 15;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0f);
    }
}
