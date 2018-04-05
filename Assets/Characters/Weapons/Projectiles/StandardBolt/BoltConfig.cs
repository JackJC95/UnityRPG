using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Projectiles/Bolt Projectile"))]
    public class BoltConfig : ProjectileConfig
    {
        [Header("Bolt Projectile Specific")]
        [SerializeField] float damage = 50f;
        [SerializeField] GameObject impactParticles;

        public override ProjectileBehaviour AddBehaviourComponent(GameObject projectile)
        {
            return projectile.AddComponent<BoltBehaviour>();
        }

        public float GetDamage()
        {
            return damage;
        }

        public GameObject GetImpactParticles()
        {
            return impactParticles;
        }
    }
}
