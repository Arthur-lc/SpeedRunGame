using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : EnemyBase
{
    [SerializeField] private float speed;

    //override protected void BehaviourUpdate(float delta) {
    private void Update() {
        if(IsOnEdge() || IsOnWall()) {
            Flip(-horizontalDir);
        }

        transform.Translate(horizontalDir * speed * Time.deltaTime, 0, 0);
    }
}
