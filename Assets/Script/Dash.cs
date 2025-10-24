using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerController;
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.5f;
    public float dashingCooldown = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(CharacterDash());
        }
    }

    private IEnumerator CharacterDash()
    {
        canDash = false;
        isDashing = true;
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput > 0)
        {
            horizontalInput = transform.localScale.x > 0 ? 1 : -1;
        }
        if (verticalInput > 0)
        {
            verticalInput = transform.localScale.y > 0 ? 1 : -1;
        }
        rb.velocity = new Vector2(horizontalInput * dashingPower, verticalInput * dashingPower);

        yield return new WaitForSeconds(dashingTime);
        if (playerController != null)
        {
            playerController.OnDashEnd();
        }
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public bool IsDashing => isDashing;
}
