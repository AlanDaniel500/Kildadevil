using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Charac_movement : MonoBehaviour
{
    [Header("Player componet reference")]
    [SerializeField] Rigidbody2D rb;


    [Header("Player Settings")]
    [SerializeField] float moveSpeed;

    private float horizontal;

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
  
}
