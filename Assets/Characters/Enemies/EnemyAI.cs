using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof (Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;

        enum State { idle, patrolling, chasing, attacking }
        State state = State.idle;

        float currentWeaponRange;
        float distanceToPlayer;
        PlayerMovement player = null;
        Character character;

        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
            character = GetComponent<Character>();
        }

        private void Update()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                state = State.patrolling;
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        void OnDrawGizmos()
        {
            // Draw attack spheres
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }       
    }
}
