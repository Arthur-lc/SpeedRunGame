using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private Transform feet;
    [SerializeField] private Transform head;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float drag;

    const float Gravity = 8.9f;

    private Vector2 velocity;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveToward(Vector2 direction)
    {
        Move(direction * speed);
    }

    public void Move(Vector2 velocity)
    {
        this.velocity += velocity * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
        if (velocity.x > 0)
        {
            velocity.x -= speed * drag * Time.fixedDeltaTime;
        } 
        else if (velocity.x < 0)
        {
            velocity.x += speed * drag * Time.fixedDeltaTime;
        }
    }
}
