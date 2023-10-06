using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    public float HorizontalDir { get; private set;}
    private Vector2 velocity;

    
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
    private float DashSpeed => dashDistance / dashDuration;


    [Header("Checks")]
    [SerializeField] private LayerMask groundLayer;
    private bool collisionUp, collisionDown;

    [Header("Ground Check")]
    [SerializeField] private Vector3 lowerCollisionBound;
    [SerializeField] private float groundDitstance;
    public bool IsOnFLoor => collisionDown && velocity.y <= 0; // esta no chao se colidindo com o chao e nao esta subindo
    private bool wasOnAir; // estava no ar no ultimo frame
    public bool isLanding => IsOnFLoor && wasOnAir; // estava no ar e tocou o chao NESTE frame

    [Header("Cealing Check")]
    [SerializeField] private Vector3 upperCollisionBound;
    [SerializeField] private float cealingDitstance;
    public bool IsOnCealing => collisionUp && velocity.y >= 0;

    private float gravity;
    private bool isGravityEnabled = true;
    private Collider2D coll;


    private void Start() {
        coll = GetComponent<Collider2D>();
    }

    private void Update() {
        HorizontalDir = 0;
        HorizontalDir = Input.GetAxisRaw("Horizontal");

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space)) {
            velocity.y = jumpForce;
            gravity = 0;
        }
    }

    private void FixedUpdate()
    {
        HandleColisions();

        if (isDashing)
            UpdateDash();
        else {    
            if (IsOnCealing) {
                velocity.y = -velocity.y;
            }

            if (IsOnFLoor) {
                gravity = 0f;
                velocity.y = 0f;
                velocity.x = HorizontalDir * speed;
            }
            else {
                velocity.x += HorizontalDir * speed * airControl;
                velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
                if (isGravityEnabled) {
                    gravity += Gravity * Time.fixedDeltaTime;
                    velocity.y -= gravity;
                }
            }   
        }
        
        transform.Translate(velocity * Time.fixedDeltaTime);

    }

    /// <summary>
    /// Executa o dash caso outro dash não estaja em execução
    /// </summary>
    /// <param name="direction">direção do dash (precisar ser normalizado)</param>
    /// <param name="distance">distancia percorrida durante o dash</param>
    /// <param name="duration">tempo para percorrer a distancia do dash</param>
    public void Dash(Vector2 direction, float distance, float duration) {
        if (isDashing)
            return;
        
        dashDir = direction;
        dashDistance = distance;
        dashDuration = duration;

        isDashing = true;
        timeSinceDash = 0;
    }

    private void UpdateDash() { // chamar no fixedUpdate
        timeSinceDash += Time.fixedDeltaTime;
        Debug.Log(IsOnFLoor);
        velocity = dashDir * DashSpeed;
        if(timeSinceDash >= dashDuration || isLanding) {
            isDashing = false;
            isGravityEnabled = true;
            gravity = 0f;
            velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
            velocity.y = Mathf.Clamp(velocity.y, -speed, speed);

        }
    }

    private void HandleColisions() {
        lowerCollisionBound.x = coll.bounds.center.x;
        lowerCollisionBound.y = coll.bounds.center.y - coll.bounds.extents.y;
        upperCollisionBound.x = coll.bounds.center.x;
        upperCollisionBound.y = coll.bounds.center.y + coll.bounds.extents.y;


        wasOnAir = !IsOnFLoor;

        collisionUp = Physics2D.Raycast(upperCollisionBound, Vector2.down, cealingDitstance, groundLayer);
        collisionDown = Physics2D.Raycast(lowerCollisionBound, Vector2.down, groundDitstance, groundLayer);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = lowerCollisionBound;
        Vector3 to = from + Vector3.down * groundDitstance;
        Gizmos.DrawLine(from, to);

        from = upperCollisionBound;
        to = from + Vector3.up * groundDitstance;
        Gizmos.DrawLine(from, to);
    }
}
