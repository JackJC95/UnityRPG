using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }

        void Start()
        {
            print("Area Effect behaviour attached to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            print("Area effect called, damage: " + config.GetAreaDamage() + " , radius: " + config.GetRadiusOfEffect());
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadiusOfEffect(), Vector3.up, config.GetRadiusOfEffect());

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float damageToDeal = useParams.baseDamage + config.GetAreaDamage();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }
}
