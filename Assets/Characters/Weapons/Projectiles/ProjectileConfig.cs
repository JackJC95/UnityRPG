using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class ProjectileConfig : ScriptableObject
    {
        [Header("Projectile General")]
        [SerializeField] float launchSpeed;
        [SerializeField] GameObject projectilePrefab = null;
        [SerializeField] AudioClip launchSound = null;

        protected ProjectileBehaviour behaviour;

        public abstract ProjectileBehaviour AddBehaviourComponent(GameObject projectile);  

        public void FireProjectile(Vector3 target, Vector3 spawnPosition)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            ProjectileBehaviour behaviourComponent = AddBehaviourComponent(projectile);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
            var rigidBody = projectile.AddComponent<Rigidbody>();
            Vector3 direction = (target - spawnPosition).normalized;
            rigidBody.velocity = direction * launchSpeed;
        }

        public float GetLaunchSpeed()
        {
            return launchSpeed;
        }

        public GameObject GetProjectilePrefab()
        {
            return projectilePrefab;
        }

        public AudioClip GetLaunchSound()
        {
            return launchSound;
        }
    }
}
