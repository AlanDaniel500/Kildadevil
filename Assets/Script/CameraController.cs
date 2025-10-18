using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector2 limitsX = new Vector2(-10f, 10f);

    void LateUpdate()
    {
        if (player == null) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(player.position.x, limitsX.x, limitsX.y);
        transform.position = pos;
    }

}
