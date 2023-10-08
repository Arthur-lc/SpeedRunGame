using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("edge/wall detection")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float detectionRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    protected float horizontalDir = -1;
    private Vector3 EdgeDectionPos => transform.position + new Vector3(horizontalDir * detectionRange, -GetComponent<Collider2D>().bounds.extents.y);
    private Vector3 WallDetctionPos => transform.position + new Vector3(horizontalDir * detectionRange, 0);

    protected bool IsOnEdge() {
        Collider2D hit = Physics2D.OverlapCircle(EdgeDectionPos, detectionRadius, groundLayer);
        return !hit;
    }

    protected bool IsOnWall() {
        Collider2D hit = Physics2D.OverlapCircle(WallDetctionPos, detectionRadius, groundLayer);
        return hit;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(EdgeDectionPos, detectionRadius);
        Gizmos.DrawWireSphere(WallDetctionPos, detectionRadius);
    }
}
