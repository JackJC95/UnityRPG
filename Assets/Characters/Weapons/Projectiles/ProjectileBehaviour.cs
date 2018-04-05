using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        protected ProjectileConfig config;

        public void SetConfig(ProjectileConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayLaunchSound()
        {
            var launchSound = config.GetLaunchSound();
            var audioSource = FindObjectOfType<PlayerControl>().GetComponent<AudioSource>();
            audioSource.PlayOneShot(launchSound);
        }
    }
}
