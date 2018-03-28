using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 5f;
        [SerializeField] AudioClip outOfEnergy;

        float currentEnergyPoints;
        AudioSource audioSource;

        float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        private void Update()
        {
            if(currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            float energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergy);               
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        private void AddEnergyPoints()
        {
            float pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public void UpdateEnergyBar()
        {
            energyBar.fillAmount = energyAsPercent;
        }
    }
}
