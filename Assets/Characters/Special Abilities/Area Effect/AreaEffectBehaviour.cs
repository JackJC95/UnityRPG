using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;
        AudioSource audioSource = null;

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            DeaRadialDamage(useParams);
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

        private void DeaRadialDamage(AbilityUseParams useParams)
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadiusOfEffect(), Vector3.up, config.GetRadiusOfEffect());

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = useParams.baseDamage + config.GetAreaDamage();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }
}
