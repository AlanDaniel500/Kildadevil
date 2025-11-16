using UnityEngine;

public class CameraFollowWithAim : MonoBehaviour
{
    public Transform target;  // El jugador
    public float followSpeed = 5f;
    public float aimOffsetAmount = 2f;

    private PlayerController playerController;

    void Start()
    {
        playerController = target.GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Posición base: seguir al jugador
        Vector3 targetPos = target.position;

        // Por defecto la cámara NO se mueve hacia el mouse
        Vector3 offset = Vector3.zero;

        // Solo mover hacia el mouse si el jugador está en modo apuntado
        if (playerController != null && playerController.IsAiming())
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            Vector3 aimDirection = mouseWorld - target.position;
            offset = aimDirection.normalized * aimOffsetAmount;
        }

        Vector3 finalPos = targetPos + offset;
        finalPos.z = -10;

        transform.position = Vector3.Lerp(transform.position, finalPos, followSpeed * Time.deltaTime);
    }
}


