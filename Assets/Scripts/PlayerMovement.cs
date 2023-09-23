using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] private float moveSpeed = 6f; // 6f
    // Assign values to these to determine gravity and jumpVelocity
    [SerializeField] private float maxJumpHeight = 4f; // 4f
    [SerializeField] private float timeToJumpApex = 0.4f; // 0.4f
    // X Velocity Smoothing Variables
    [SerializeField] private float accelerationTimeAirborne = 0.2f; // 0.2f
    [SerializeField] private float accelerationTimeGrounded = 0.1f; // 0.1f

    // Start Variables
    Controller2D controller;
    // Determined by maxJumpHeight, timeToJumpApex
    private float jumpForce;
    private float gravity;

    // X Velocity Smoothing Variables
    private float velocityXSmoothing;
    private float targetVelocityX;

    // Faster Falling Variables
    private float gravityDown;
    private bool reachedApex = true;
    private float maxHeightReached = Mathf.NegativeInfinity;
    private float startHeight = Mathf.NegativeInfinity;

    // Update Variables
    private float jumpTimer = 0;
    private bool JumpButton = false;
    Vector3 velocity;
    Vector3 prevVelocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -2 * maxJumpHeight / Mathf.Pow(timeToJumpApex, 2);
        gravityDown = gravity * 2;

        jumpForce = 2 * maxJumpHeight / timeToJumpApex;
    }

    void Update()
    {
        JumpButton = Input.GetKeyDown("space");

        if (!JumpButton)
        {
            gravity = gravityDown;
        }

        if (JumpButton && controller.collisions.below)
        {
            Jump();
        }

        if (!reachedApex && maxHeightReached > transform.position.y)
        {
            reachedApex = true;
            gravity = gravityDown;
        }
        maxHeightReached = Mathf.Max(transform.position.y, maxHeightReached);


        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), 0);
        targetVelocityX = input.x * moveSpeed;
    }

    void FixedUpdate()
    {
        if (!controller.collisions.below && !reachedApex)
        {
            jumpTimer += Time.fixedDeltaTime;
        }

        prevVelocity = velocity;

        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.fixedDeltaTime;
        Vector3 deltaPosition = (prevVelocity + velocity) * 0.5f * Time.fixedDeltaTime;
        controller.Move(deltaPosition);

        // Removes the accumulation of gravity
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        // Removes the continuous collision force left/right
        if (controller.collisions.left || controller.collisions.right)
        {
            velocity.x = 0;
        }
    }

    private void Jump()
    {
        jumpTimer = 0;
        velocity.y = jumpForce;

        // Used for faster falling
        gravity = -2 * maxJumpHeight / Mathf.Pow(timeToJumpApex, 2);
        reachedApex = false;
        maxHeightReached = Mathf.NegativeInfinity;
        startHeight = transform.position.y;
    }
}
