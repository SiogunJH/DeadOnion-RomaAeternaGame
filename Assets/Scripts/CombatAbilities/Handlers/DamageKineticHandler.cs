using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKineticHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.DamageKinetic;

    protected override void DoEffect(CombatAbilityEffect effect, Character affectedCharacter)
    {
        affectedCharacter.TakeElementalDamage(effect.Amount, effect.Type);
    }
}
