using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private float speed;
    [SerializeField] private float duration = 4f;
    //[SerializeField] private float damage = 1f;

    [NonSerialized] public Vector2 direction;

    private float timeAlive = 0;

    private void Update() {
        timeAlive += Time.deltaTime;
        if (timeAlive > duration)
            Destroy(gameObject);

        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // if (other.collider.includeLayers == ignoreLayer)
        //     return;

        // if(other.collider.CompareTag("Player")) {
        // }
        Destroy(gameObject);
    }

}
