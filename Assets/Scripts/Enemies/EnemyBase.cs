using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    right = 1,
    left = -1
}

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Direction startFacingDirection;

    [Header("edge/wall detection")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float detectionRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    protected int horizontalDir = -1;
    private Vector3 EdgeDectionPos => transform.position + new Vector3(horizontalDir * detectionRange, -GetComponent<Collider2D>().bounds.extents.y);
    private Vector3 WallDetctionPos => transform.position + new Vector3(horizontalDir * detectionRange, 0);

    private void Start() {
        Flip((int)startFacingDirection);
    }

    protected bool IsOnEdge() {
        Collider2D hit = Physics2D.OverlapCircle(EdgeDectionPos, detectionRadius, groundLayer);
        return !hit;
    }

    protected bool IsOnWall() {
        Collider2D hit = Physics2D.OverlapCircle(WallDetctionPos, detectionRadius, groundLayer);
        return hit;
    }

    protected Vector2 GetFacingDirection() {
        if (horizontalDir == 1)
            return Vector2.right;
        else
            return Vector2.left;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(EdgeDectionPos, detectionRadius);
        Gizmos.DrawWireSphere(WallDetctionPos, detectionRadius);
    }

    protected void Flip(int dir) {
        if ((horizontalDir > 0 && dir == (int)Direction.left) || (horizontalDir < 0 && dir == (int)Direction.right)) {
            Vector3 currentScale = transform.localScale;
            currentScale.x += -1;
            horizontalDir = dir;
        }
    }
}
