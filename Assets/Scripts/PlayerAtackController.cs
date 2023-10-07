using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtackController : MonoBehaviour
{
    [Header("Sword")]
    // [SerializeField] private float swardDamage;
    [SerializeField] private float swordDashDistance;
    [SerializeField] private float swordDashDuration;

    [Header("Gun")]
    // [SerializeField] private float gunDamage;
    [SerializeField] private float gunDashDistance;
    [SerializeField] private float gunDashDuration;
    [SerializeField] [Range(0,10)] private float keyboardInfluence;

    
    private PlayerMovement playerMovement;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update() {
        // espada
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SwordAtack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            GunAtack();
        }
    }

    void SwordAtack() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDir = mousePos - (Vector2)transform.position;

        if (playerMovement.IsOnFLoor) {
            dashDir.y = Mathf.Max(dashDir.y, 0);
        }
        dashDir.Normalize();

        playerMovement.Dash(dashDir, swordDashDistance, swordDashDuration);

        Vector3 from = transform.position;
        Vector3 to = from + (Vector3)dashDir * swordDashDistance;
        Debug.DrawLine(from, to, Color.green, swordDashDuration *2 );
    }

    void GunAtack() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDir = -(mousePos - (Vector2)transform.position);
        
        dashDir.x += playerMovement.HorizontalDir * keyboardInfluence; // mistura a direção do mouse com a do teclado
        dashDir.Normalize();

        if (!playerMovement.IsOnFLoor || dashDir.y > 0) { // se estiver no chao e atirar pra cima, não executa o dash (talvez aumentar um pouco o 0 seja interessante)
            playerMovement.Dash(dashDir, gunDashDistance, gunDashDuration);
        }

        // Debug
        Vector3 from = transform.position;
        Vector3 to = from - (Vector3)dashDir * gunDashDistance;
        Debug.DrawLine(from, to, Color.yellow, gunDashDuration *2 );
    }
}