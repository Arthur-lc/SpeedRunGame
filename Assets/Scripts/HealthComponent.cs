using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
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
