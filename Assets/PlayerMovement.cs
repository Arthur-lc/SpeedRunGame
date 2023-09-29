using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDir;
    private Vector2 velocity;
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float Gravity = 10f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private bool isDashing = false;
    private float timeSinceLastDash = 0;


    [Header("Checks")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundDitstance;
    private bool isOnFLoor = false;

    [Header("Cealing Check")]
    [SerializeField] private Transform cealingCheckTransform;
    [SerializeField] private float cealingDitstance;
    private bool isOnCealing = false;

    private float gravity;
    private bool isGravityEnabled = true;

    private void Update() {
        moveDir = Vector2.zero;
        moveDir.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)) {
            velocity.y += jumpForce;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) { // tirar o isDashing quando implementar o cooldown
            isDashing = true;
            timeSinceLastDash = 0;
        }
    }

    private void FixedUpdate()
    {
        isOnFLoor = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundDitstance, groundLayer);
        isOnCealing = Physics2D.Raycast(cealingCheckTransform.position, Vector2.down, cealingDitstance, groundLayer);

        if (isOnCealing) {
            velocity.y = 0;
        }

        if (isOnFLoor && velocity.y <= 0) {
            gravity = 0f;
            velocity.y = 0f;
        }
        else if (isGravityEnabled) {
            gravity += Gravity * Time.fixedDeltaTime;
            velocity.y -= gravity;
        }   


        velocity.x = 0;
        if (isDashing)
            Dash();
        velocity += moveDir * speed * Time.fixedDeltaTime;
        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    private void Dash() {
        velocity.x = moveDir.x * dashForce;
        timeSinceLastDash += Time.fixedDeltaTime;
        isGravityEnabled = false;
        if(timeSinceLastDash >= dashTime) {
            isDashing = false;
            isGravityEnabled = true;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = groundCheckTransform.position;
        Vector3 to = from + Vector3.down * groundDitstance;
        Gizmos.DrawLine(from, to);

        from = cealingCheckTransform.position;
        to = from + Vector3.up * groundDitstance;
        Gizmos.DrawLine(from, to);
    }
}
