using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerMovement player = null;

        private void Start()
        {
            player = GetComponent<PlayerMovement>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetHealAmount());
            PlayParticleEffect();
            PlayAbilityAnimation();
        }
    }
}
