using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 8f;
    [SerializeField] float chaseRadius = 6f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float secondsBetweenShots = 0.5f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    bool isAttacking = false;
    float currentHealthPoints = 100f;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }

        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); // TODO switch to coroutines
        }

        if (distanceToPlayer > attackRadius)
        {
            isAttacking = false;
            CancelInvoke();
        }
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);

        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
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
