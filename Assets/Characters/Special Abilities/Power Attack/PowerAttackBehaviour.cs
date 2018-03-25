using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;
        AudioSource audioSource = null;

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            DealPowerAttackDamage(useParams);
            PlayParticleEffect();
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            GameObject prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

        private void DealPowerAttackDamage(AbilityUseParams useParams)
        {
            print("Power attack used by: " + gameObject.name);
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}
