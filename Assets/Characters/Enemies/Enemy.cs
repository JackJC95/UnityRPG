using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO consider rewire
using RPG.Core; 

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float attackRadius = 8f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;

        bool isAttacking = false;
        Player player = null;

        private void Start()
        {
            player = GameObject.FindObjectOfType<Player>();
            currentHealthPoints = maxHealthPoints;
        }

        private void Update()
        {
            if(player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this); // to stop enemy behaviour
            }
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= chaseRadius)
            {
                // aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                // aiCharacterControl.SetTarget(transform);
            }

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodInSeconds + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);
            }

            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }
        }

        // TODO separate out character firing logic
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnDrawGizmos()
        {
            // Draw attack spheres
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw chase sphere
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
