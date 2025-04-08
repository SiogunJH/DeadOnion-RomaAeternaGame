using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Heal;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        if (targetCharacter == null) return;

        int heal = Random.Range(effect.MinMax.x, effect.MinMax.y + 1);
        targetCharacter.Heal(heal);
    }
}
