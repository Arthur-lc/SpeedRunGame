using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyBase
{
    [SerializeField] private float fireCooldown = 5f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask playerLayer;

    private float timeSinceFire = 0f;

    void Update()
    {
        if (timeSinceFire > fireCooldown) {
            Fire();
        }

        timeSinceFire += Time.deltaTime;
    }

    void Fire() {
        timeSinceFire = 0f;
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        //newProjectile.transform.LookAt(newProjectile.transform.position + new Vector3((int)FacingDirection, 0, 0));
        Debug.Log(newProjectile.transform.right);

        newProjectile.SetActive(true);
    }
}
