using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private PlayerController playerController;
    public float followSpeed = 5f;
    public float aimOffsetAmount = 2f;
    public Vector2 limitsX = new Vector2(-10f, 10f);
    private Vector3 targetPosition; // Posición objetivo actual (suavizada)

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("PlayerController no encontrado en el target.");

        if (player != null)
        {
            Vector3 startPos = transform.position;
            startPos.x = Mathf.Clamp(player.position.x, limitsX.x, limitsX.y);
            startPos.z = -10f;
            transform.position = startPos;
        }
    }
    void LateUpdate()
    {
        if (player == null || playerController == null) return;

        if (FindFirstObjectByType<BossController>() != null)
        {
            return;
        }

        Vector3 currentPos = transform.position;

        if (playerController.IsAiming())
        {
            // === MODO APUNTADO ===
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -currentPos.z; // Distancia correcta desde la cámara
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            float aimDirectionX = mouseWorld.x - player.position.x;
            float offsetX = Mathf.Sign(aimDirectionX) * aimOffsetAmount;

            // Posición objetivo: jugador + offset, pero respetando límites
            float desiredX = player.position.x + offsetX;
            desiredX = Mathf.Clamp(desiredX, limitsX.x, limitsX.y);

            targetPosition = new Vector3(desiredX, currentPos.y, -10f);
        }
        else
        {
            // === MODO NORMAL: volver al jugador centrado (con límites) ===
            float clampedX = Mathf.Clamp(player.position.x, limitsX.x, limitsX.y);
            targetPosition = new Vector3(clampedX, currentPos.y, -10f);
        }

        // === APLICAR MOVIMIENTO SUAVE EN AMBOS MODOS ===
        transform.position = Vector3.Lerp(currentPos, targetPosition, followSpeed * Time.deltaTime);

    }

}
