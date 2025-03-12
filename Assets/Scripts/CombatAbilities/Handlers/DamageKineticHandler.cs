using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKineticHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.DamageKinetic;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        if (targetCharacter == null) return;

        targetCharacter.TakeElementalDamage(effect.Amount, effect.Type);
    }
}
