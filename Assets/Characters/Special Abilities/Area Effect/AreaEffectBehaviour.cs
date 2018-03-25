using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : AbilityBehaviour
    {

        public override void Use(AbilityUseParams useParams)
        {
            PlayAbilitySound();
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, 
                (config as AreaEffectConfig).GetRadiusOfEffect(), 
                Vector3.up, 
                (config as AreaEffectConfig).GetRadiusOfEffect());

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).GetAreaDamage();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }
}
