using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType{
    Continuous,
    FollowPlayer,
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CameraType cameraType;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

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

    void MoveCamera()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if(player.transform.position.x < transform.position.x  -halfWidth){
        	Debug.Log("O jogador morreu");
        	player.gameObject.SetActive(false);
        }

    }

    void FollowPlayer()
    {
        var input = Input.GetAxis("Horizontal");
        float xPos = Mathf.Clamp(player.position.x, leftEdge.position.x + halfWidth, rightEdge.position.x - halfWidth);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        switch(cameraType)
        {
            case CameraType.Continuous: MoveCamera(); break;
            case CameraType.FollowPlayer: FollowPlayer(); break;
        }
    }
}
