using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private float speed;
    //[SerializeField] private float damage = 1f;

    private Vector2 direction;

    private void Update() {
        transform.Translate(speed * Time.deltaTime * transform.right);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.includeLayers == ignoreLayer)
            return;

        if(other.collider.includeLayers == targetLayer) {
            Debug.Log("Bateu");
        }
        
        Destroy(gameObject);
    }
}
