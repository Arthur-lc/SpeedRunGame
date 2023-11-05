using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerAttackController : MonoBehaviour
{
    // Ataque do personagem, onde ele pode usar uma espada e uma arma, ambas dão um dash, a espada para frente e a arma para trás
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask enemyAndGroundLayer;

    [Header("Sword")]
    [SerializeField] private float swordDamage;
    [SerializeField] private float swordRadius;
    [SerializeField] private float swordAimAssistRadius = 1f;
    [SerializeField] private float swordDashDistance;
    [SerializeField] private float swordDashDuration;
    [SerializeField] private bool swordNeedTarget = true;

    [Header("Gun")]
    [SerializeField] private float gunDamage;
    [SerializeField] private float gunRange;
    [SerializeField] private float gunRadius;
    [SerializeField] private float gunDashDistance;
    [SerializeField] private float gunDashDuration;
    [SerializeField] [Range(0,10)] private float keyboardInfluence;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private ParticleSystem gunParticleSystem;


    private PlayerMovement playerMovement;
    
    private Vector2 MouseDir => ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
    private float timeSinceAtacked = 10f; // usando so pra debug

    private AudioSource gunSound;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        gunSound = GetComponents<AudioSource>()[2];
    }

    void Update() {
        // espada
        timeSinceAtacked += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SwordAtack();
            timeSinceAtacked = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            GunAtack();
            gunSpriteRenderer.enabled = true;
            timeSinceAtacked = 0f;
        }
    }

    void SwordAtack() {
        // get Dir
        Vector2 attackDir = MouseDir;

        // Aim Assist
        Vector2 target = GetTarget(attackDir, swordAimAssistRadius, swordDashDistance);
        if (target != Vector2.zero)
            attackDir = target - (Vector2)transform.position;
        
        attackDir.Normalize();


        // Ataque
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, attackDir, swordDashDistance, enemyAndGroundLayer);
        if (raycastHit && raycastHit.collider.CompareTag("Enemy")) {
            HealthComponent targetHealthComponent = raycastHit.collider.gameObject.GetComponent<HealthComponent>();
            if (targetHealthComponent) {
                targetHealthComponent.Damage(swordDamage);
                
                //Dash
                playerMovement.Dash(attackDir, swordDashDistance, swordDashDuration);

                Vector3 from = transform.position;
                Vector3 to = from + (Vector3)attackDir * swordDashDistance;
                Debug.DrawLine(from, to, Color.green, swordDashDuration *2 );
            }
        }
        else if(!swordNeedTarget) {
            playerMovement.Dash(attackDir, swordDashDistance, swordDashDuration);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    void GunAtack() {
        StartCoroutine(ShowGun());
        gunParticleSystem.Play();

        Vector2 attackDir = MouseDir;
        Vector2 dashDir = -attackDir;
        
        dashDir.x += playerMovement.HorizontalDir * keyboardInfluence; // mistura a direção do mouse com a do teclado
        dashDir.Normalize();
        
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, gunRadius, attackDir, gunRange, enemyAndGroundLayer);
        if (hit) {
            HealthComponent targetHealthComponent = hit.collider.gameObject.GetComponent<HealthComponent>();
            if (targetHealthComponent) {
                targetHealthComponent.Damage(swordDamage);
            }
        }

        if (!playerMovement.IsOnFLoor) {
            playerMovement.Dash(dashDir, gunDashDistance, gunDashDuration);
        }

        gunSound.Play();
        
        // Debug
        Vector3 from = transform.position;
        Vector3 to = from + (Vector3)attackDir * gunRange;
        Debug.DrawLine(from, to, Color.green, gunDashDuration *2);
    }

    Vector2 GetTarget(Vector2 dir, float assistRadius, float distance) {
        RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, assistRadius, dir, distance, enemyLayer);
        
        if (raycastHit) {
            Debug.DrawLine(transform.position, raycastHit.collider.transform.position, Color.yellow, gunDashDuration *2);
            return raycastHit.collider.transform.position;
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)dir * distance, Color.yellow, gunDashDuration *2);
        return Vector2.zero;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        if (timeSinceAtacked < .5f)
            for (int i = 0; i <= 10; i++)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)(i * swordDashDistance * MouseDir /10), swordAimAssistRadius);
                
            }
    }

    IEnumerator ShowGun()
    {
        gunSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(gunDashDuration*3);
        gunSpriteRenderer.enabled = false;
    }
}