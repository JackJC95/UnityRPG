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

    private void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        print("Damageable component = " + damageableComponent);
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }
}
