using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDir;
    private Vector2 velocity;
    private Vector2 oldVelocity; // pra guardar a velocidade antes do dash

    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] [Range(0f, 1f)] private float airControl = 0.1f;
    [SerializeField] private float Gravity = 10f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    private bool isDashing = false;
    private float timeSinceDash = 0;
    private Vector2 dashDir;
    private float dashSpeed => dashDistance / dashDuration;


    [Header("Checks")]
    [SerializeField] private LayerMask groundLayer;
    private bool collisionUp, collisionDown;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundDitstance;
    private bool isOnFLoor => collisionDown && velocity.y <= 0; // esta no chao se colidindo com o chao e nao esta subindo
    private bool wasOnAir; // estava no ar no ultimo frame
    private bool isLanding => isOnFLoor && wasOnAir; // estava no ar e tocou o chao NESTE frame

    [Header("Cealing Check")]
    [SerializeField] private Transform cealingCheckTransform;
    [SerializeField] private float cealingDitstance;
    private bool isOnCealing => collisionUp && velocity.y >= 0;

    private float gravity;
    private bool isGravityEnabled = true;

    private void Update() {
        moveDir = Vector2.zero;
        moveDir.x = Input.GetAxisRaw("Horizontal");

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space)) {
            velocity.y = jumpForce;
            gravity = 0;
        }

        // Espada
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDashing) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDir = (mousePos - (Vector2)transform.position);
            if (isOnFLoor) {
                dashDir.y = Mathf.Clamp(dashDir.y, 0, 1);
            }
            dashDir.Normalize();

            isDashing = true;
            timeSinceDash = 0;
            oldVelocity = velocity;

            Vector3 from = transform.position;
            Vector3 to = from + (Vector3)dashDir * dashDistance;
            Debug.DrawLine(from, to, Color.green, dashDuration *2 );
        }

        // Arma
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDashing) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDir = -(mousePos - (Vector2)transform.position);
            dashDir += moveDir; // mistura a direção do mouse com a do teclado
            dashDir.Normalize();

            if (!isOnFLoor || dashDir.y > 0) { // se estiver no chao e atirar pra cima, não executa o dash (talvez aumentar um pouco o 0 seja interessante)
                isDashing = true;
                timeSinceDash = 0;

                Vector3 from = transform.position;
                Vector3 to = from - (Vector3)dashDir * dashDistance;
                Debug.DrawLine(from, to, Color.yellow, dashDuration *2 );
            }
        }
    }

    private void FixedUpdate()
    {
        HandleColisions();

        if (isDashing)
            Dash();
        else {    
            if (isOnCealing) {
                velocity.y = -velocity.y;
            }

            if (isOnFLoor) {
                gravity = 0f;
                velocity.y = 0f;
                velocity.x = moveDir.x * speed;
            }
            else {
                velocity.x += moveDir.x * speed * airControl;
                velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
                if (isGravityEnabled) {
                    gravity += Gravity * Time.fixedDeltaTime;
                    velocity.y -= gravity;
                }
            }   
        }
        
        transform.Translate(velocity * Time.fixedDeltaTime);

    }

    private void Dash() {
        timeSinceDash += Time.fixedDeltaTime;
        Debug.Log(isOnFLoor);
        velocity = dashDir * dashSpeed;
        if(timeSinceDash >= dashDuration || isLanding) {
            isDashing = false;
            isGravityEnabled = true;
            gravity = 0f;
            velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
            velocity.y = Mathf.Clamp(velocity.y, -speed, speed);

        }
    }

    private void HandleColisions() {
        wasOnAir = !isOnFLoor;

        collisionUp = Physics2D.Raycast(cealingCheckTransform.position, Vector2.down, cealingDitstance, groundLayer);
        collisionDown = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundDitstance, groundLayer);
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
