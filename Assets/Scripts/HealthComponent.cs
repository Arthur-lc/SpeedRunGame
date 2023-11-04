using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    // Classe para os inimigos, com uma vida máxima, e o hp atual, com uma função Damage, que quando chamada tira a vida do inimigo e uma 
    // função Die que destroi o inimigo
    [SerializeField] private float MaxHp;
    public float CurrentHp {get; private set;}

    private void Start() {
        CurrentHp = MaxHp;
    }

    public void Damage(float damage) {
        CurrentHp -= damage;
        
        if (CurrentHp <= 0)
            Die();
    }

    private void Die() {
        Destroy(gameObject);
    }
    
}
