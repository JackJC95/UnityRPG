using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;
using System;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;

        const string ATTACK_TRIGGER = "Attack";
        float lastHitTime;  

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();        
        }

        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;
            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                float targetHealthPercentage = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealthPercentage <= Mathf.Epsilon;
                float targetRange = Vector3.Distance(target.transform.position, character.transform.position);
                targetIsOutOfRange = targetRange > currentWeaponConfig.GetMaxAttackRange();
            }

            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = (characterHealth <= Mathf.Epsilon);
            if (characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        public void StopAttacking()
        {
            animator.StopPlayback();
            StopAllCoroutines();
        }

        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                // know how often to attack
                var animationClip = currentWeaponConfig.GetAttackAnimClip();
                float animationClipTime = animationClip.length / character.GetAnimSpeedMultiplier();
                float timeToWait = animationClipTime + currentWeaponConfig.GetTimeBetweenAnimCycles();
                // print(timeToWait);
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                
                if (isTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }            
        }

        void AttackTargetOnce()
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = currentWeaponConfig.GetDamageDelay();
            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float damageDelay)
        {
            yield return new WaitForSecondsRealtime(damageDelay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }

        public WeaponConfig GetCurrentWeapon() { return currentWeaponConfig; }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No dominant hand found on player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple dominant hand scripts on player, please remove one");
            return dominantHands[0].gameObject;
        }

        void SetAttackAnimation()
        {
            if (!character.GetOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Please proved " + gameObject + " with an animtator override controller");
            }
            else
            {
                var animatorOverrideController = character.GetOverrideController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController["DEFAULT ATTACK"] = currentWeaponConfig.GetAttackAnimClip();
            }
        }        

        float CalculateDamage()
        {
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();            
        }

        public Animator GetAnimator()
        {
            return animator;
        }
    }
}
