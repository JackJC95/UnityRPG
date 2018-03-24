using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Special Ability General")]
        [SerializeField] float extraDamage = 10f;

        public override ISpecialAbility AddComponent(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            return behaviourComponent;
        }
    }
}
