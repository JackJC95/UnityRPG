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
            PlayAbilityAnimation();
            PlayParticleEffect();
            StartCoroutine(ExecuteAfterSeconds(target));                       
        }

        IEnumerator ExecuteAfterSeconds(GameObject target)
        {
            yield return new WaitForSeconds((config as PowerAttackConfig).GetEffectDelay());
            DealPowerAttackDamage(target);
            PlayAbilitySound();
        }

        private void DealPowerAttackDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
