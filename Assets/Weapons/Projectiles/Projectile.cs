using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damageCaused;
    public float projectileSpeed;

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, 0.01f);
    }
}
