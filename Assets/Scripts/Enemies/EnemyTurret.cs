using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyBase
{
    [SerializeField] private float fireCooldown = 5f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask playerLayer;

    private float timeSinceFire = 0f;

    private AudioSource shootSound;

    private void Start() {
        shootSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timeSinceFire > fireCooldown) {
            Fire();
        }

        timeSinceFire += Time.deltaTime;
    }

    void Fire() {
        timeSinceFire = 0f;
        Vector3 newpos = new Vector3(1.3f,0,0);
        GameObject newProjectile = Instantiate(projectile, transform.position-newpos, transform.rotation);

        newProjectile.GetComponent<Projectile>().direction = GetFacingDirection();

        newProjectile.SetActive(true);
        shootSound.Play();
    }
}
