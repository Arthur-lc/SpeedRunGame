using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Aqui temos o projetil, ele tem uma duração e uma colisão que o destrói
    [SerializeField] private float speed;
    [SerializeField] private float duration = 4f;

    [NonSerialized] public Vector2 direction;

    private float timeAlive = 0;

    private void Update() {
        timeAlive += Time.deltaTime;
        if (timeAlive > duration)
            Destroy(gameObject);

        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }

}
