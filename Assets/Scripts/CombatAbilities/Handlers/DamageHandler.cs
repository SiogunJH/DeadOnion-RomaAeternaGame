using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Damage;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        if (targetCharacter == null) return;

        targetCharacter.TakeDamage(effect.Amount);
    }
}
