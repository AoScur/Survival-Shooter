using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    [field:SerializeField]
    public float health { get; protected set; }
    [field: SerializeField]
    public bool dead { get; protected set; }
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public void ApplyUpdatedHealth(float newHP, bool newDead)
    {
        health = newHP;
        dead = newDead;
    }

    public virtual void Die()
    {
        if(onDeath!=null)
        {
            onDeath();
        }

        dead = true;
    }

    public virtual void OnDamage(float damage,Vector3 hitPoint, Vector3 hitNormal)
    {
        health= damage;

        if(health<=0&&!dead)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
        {
            return;
        }
        health = newHealth;
    }
}
