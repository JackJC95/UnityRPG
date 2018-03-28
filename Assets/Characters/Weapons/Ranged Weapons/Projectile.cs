using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // so can inspect when paused

        const float DESTROY_DELAY = 0.01f;
        float damageCaused;

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer)
            {
                // DamageIfDamageable(collision);
            }
        }

        // TODO re-implement projectiles

        //private void DamageIfDamageable(Collision collision)
        //{
        //    Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        //    if (damageableComponent)
        //    {
        //        (damageableComponent as IDamageable).TakeDamage(damageCaused);
        //    }
        //    Destroy(gameObject, DESTROY_DELAY);
        //}
    }
}
