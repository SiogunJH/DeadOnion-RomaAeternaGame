using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Damage;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        if (targetCharacter == null) return;

        int damage = Random.Range(effect.MinMax.x, effect.MinMax.y + 1);
        targetCharacter.TakeDamage(damage);
    }
}
