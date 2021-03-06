﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof (HealthSystem))]
    [RequireComponent(typeof (Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] float waypointDwellTime = 1.0f;

        enum State { idle, patrolling, chasing, attacking }
        State state = State.idle;

        float currentWeaponRange;
        float distanceToPlayer;
        int nextWaypointIndex;

        PlayerControl player = null;
        Character character;

        private void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
        }

        private void Update()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange;
            bool inChaseCircle = distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius;
            bool outsideChaseCircle = distanceToPlayer > chaseRadius && distanceToPlayer > currentWeaponRange;

            if (outsideChaseCircle)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(Patrol());
            }
            if (inChaseCircle)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(ChasePlayer());
            }
            if (inWeaponCircle)
            {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }
        }
        
        IEnumerator Patrol()
        {
            state = State.patrolling;
            
            while (patrolPath != null)
            {
                Vector3 nextWaypointPosition = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypointWhenClose(nextWaypointPosition);
                yield return new WaitForSeconds(waypointDwellTime);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPosition)
        {
            if (Vector3.Distance(transform.position, nextWaypointPosition) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }       
    }
}
