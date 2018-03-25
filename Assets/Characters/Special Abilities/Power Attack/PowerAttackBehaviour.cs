using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;        

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        void Start()
        {
            print("Power Attack behaviour attached to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            DealPowerAttackDamage(useParams);
            PlayParticleEffect();
        }

        private void PlayParticleEffect()
        {
            GameObject prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

        private void DealPowerAttackDamage(AbilityUseParams useParams)
        {
            print("Power attack used by: " + gameObject.name);
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.AdjustHealth(damageToDeal);
        }
    }
}
