using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAcidHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.DamageAcid;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        targetCharacter.TakeElementalDamage(effect.Amount, effect.Type);
    }
}
