using System;
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
    private Vector2 velocity = Vector2.zero;

    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] [Range(0f, 1f)] private float airControl = 0.1f;
    [SerializeField] private float Gravity = 10f;

    [Header("Dash")]
    [SerializeField] private TrailRenderer trail;
    private float dashDistance;
    private float dashDuration;
    private bool isDashing = false;
    private float timeSinceDash = 0;
    private Vector2 dashDir;
    private float DashSpeed => dashDistance / dashDuration;


    [Header("Checks")]
    [SerializeField] private LayerMask groundLayer;
    private bool collisionUp, collisionDown;
    private float collisionSkinWidth = 0.1f;

    private Vector3 lowerCollisionBound;
    public bool IsOnFLoor => collisionDown && velocity.y <= 0; // esta no chao se colidindo com o chao e nao esta subindo
    private bool wasOnAir; // estava no ar no ultimo frame
    public bool IsLanding => IsOnFLoor && wasOnAir; // estava no ar e tocou o chao NESTE frame
    public bool isJumping, doubleJump = false;

    private Vector3 upperCollisionBound;
    public bool IsOnCealing => collisionUp && velocity.y >= 0;

    private float gravity;
    private bool isGravityEnabled = true;
    private Collider2D coll;

    private Animator anim;
    private Vector3 previousPosition, currentPosition;

    private void Start() {
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    private void Update() {
        HorizontalDir = 0;
        HorizontalDir = Input.GetAxisRaw("Horizontal");

        bool run = Math.Abs(HorizontalDir) > 0.001;
        if(HorizontalDir > 0){
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        else if(HorizontalDir < 0){
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }
        anim.SetBool("run",run);

        HorizontalDir = Math.Abs(HorizontalDir);

        // Debug.Log("WasOnAir: " + wasOnAir);
        // Debug.Log("IsOnFloor: " + IsOnFLoor);
        currentPosition = transform.position;
        
        if (currentPosition.y < previousPosition.y)
        {
            anim.SetBool("jump",false);
            anim.SetBool("double_jump",false);
            anim.SetBool("fall",true);
        }

        if(IsOnFLoor){
            anim.SetBool("fall",false);
            isJumping = false;
        }

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(!isJumping){
                velocity.y = jumpForce;
                gravity = 0;
                doubleJump = true;
                isJumping = true;
                anim.SetBool("jump",true);
            }
            else{
                if(doubleJump){
                    velocity.y = jumpForce;
                    gravity = 0;
                    anim.SetBool("jump",false); 
                    anim.SetBool("double_jump",true); 
                    anim.SetBool("fall",false); 
                    doubleJump = false;     
                }
            }
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
        previousPosition = currentPosition;
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

        trail.emitting = true;
    }

    private void UpdateDash() { // chamar no fixedUpdate
        timeSinceDash += Time.fixedDeltaTime;
        velocity = dashDir * DashSpeed;

        if(timeSinceDash >= dashDuration || IsLanding) { // end
            isDashing = false;
            isGravityEnabled = true;
            gravity = 0f;
            velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
            velocity.y = Mathf.Clamp(velocity.y, -speed, speed);
            trail.emitting = false;
        }
    }

    private void HandleColisions() {
        lowerCollisionBound.x = coll.bounds.center.x;
        lowerCollisionBound.y = coll.bounds.center.y - coll.bounds.extents.y;
        upperCollisionBound.x = coll.bounds.center.x;
        upperCollisionBound.y = coll.bounds.center.y + coll.bounds.extents.y;


        wasOnAir = !IsOnFLoor;

        collisionUp = Physics2D.Raycast(upperCollisionBound, Vector2.down, collisionSkinWidth, groundLayer);
        collisionDown = Physics2D.Raycast(lowerCollisionBound, Vector2.down, collisionSkinWidth, groundLayer);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = lowerCollisionBound;
        Vector3 to = from + Vector3.down * collisionSkinWidth;
        Gizmos.DrawLine(from, to);

        from = upperCollisionBound;
        to = from + Vector3.up * collisionSkinWidth;
        Gizmos.DrawLine(from, to);
    }
}
