using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : EnemyBase
{
    [SerializeField] private float speed;

    private void Update() {
        if(IsOnEdge() || IsOnWall()) {
            Flip(-horizontalDir);
        }

        transform.Translate(horizontalDir * speed * Time.deltaTime, 0, 0);
    }
}
