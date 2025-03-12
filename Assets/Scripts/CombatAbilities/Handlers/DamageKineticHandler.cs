using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKineticHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.DamageKinetic;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        if (targetCharacter == null) return;

        int initalHealth = targetCharacter.CurrentHealth;
        targetCharacter.TakeElementalDamage(effect.Amount, effect.Type);

        Debug.Log($"[{targetCharacter.UserFriendlyName}] received [{effect.Amount}] points of damage, and is now at [{targetCharacter.CurrentHealth}] health points (was [{initalHealth}]) out of [{targetCharacter.CharacterProfile.TotalHealth}] total!");
    }
}
