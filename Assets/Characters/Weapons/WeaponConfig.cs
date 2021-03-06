﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;
        public Transform projectileSpawnPoint;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float timeBetweenAnimationCycles = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = 0.5f;
        [SerializeField] ProjectileConfig projectileConfig;

        public float GetTimeBetweenAnimCycles()
        {
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        public ProjectileConfig GetProjectileConfig()
        {
            return projectileConfig;
        }

        public Transform GetProjectileSpawnPoint()
        {
            return projectileSpawnPoint;
        }

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
