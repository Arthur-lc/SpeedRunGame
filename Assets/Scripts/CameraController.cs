using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform topEdge;
    [SerializeField] private Transform bottomEdge;

    private Transform player;
    private float halfWidth;
    private float halfHeight;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }
    void FixedUpdate(){
        FollowPlayer();
    }
    void FollowPlayer()
    {
        var input = Input.GetAxis("Horizontal");
        float xPos = Mathf.Clamp(player.position.x, leftEdge.position.x + halfWidth, rightEdge.position.x - halfWidth);
        float yPos = Mathf.Clamp(player.position.y, bottomEdge.position.y + halfHeight, topEdge.position.y - halfHeight);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    } 
}
