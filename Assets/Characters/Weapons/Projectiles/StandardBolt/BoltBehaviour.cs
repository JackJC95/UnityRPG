using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class BoltBehaviour : ProjectileBehaviour
    {
        const float PARTICLE_CLEAN_UP_DELAY = 1f; // TODO add destroy after delay script to particles, this obj being destroyed before delay ends

        void Start()
        {
            PlayLaunchSound();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }

            if ((config as BoltConfig).GetImpactParticles())
            {
                PlayImpactParticles();
            }
            DealDamage(collision);
            Destroy(gameObject);
        }

        private void DealDamage(Collision collision)
        {
            var enemy = collision.gameObject.GetComponent<EnemyAI>();

            if (enemy)
            {
                enemy.GetComponent<HealthSystem>().TakeDamage((config as BoltConfig).GetDamage());
            }
        }

        protected void PlayImpactParticles()
        {
            var particlePrefab = (config as BoltConfig).GetImpactParticles();
            GameObject particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }

            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }
    }
}
