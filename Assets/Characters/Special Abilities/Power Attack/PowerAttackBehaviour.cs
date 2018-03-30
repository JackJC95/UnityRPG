using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            transform.LookAt(target.transform);
            PlayAbilitySound();
            DealPowerAttackDamage(target);
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void DealPowerAttackDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
