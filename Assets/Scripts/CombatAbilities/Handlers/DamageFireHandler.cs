using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFireHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Heal;

    protected override void DoEffect(CombatAbilityEffect effect, Character affectedCharacter)
    {
        affectedCharacter.TakeElementalDamage(effect.Amount, effect.Type);
    }
}
