using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKineticHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.DamageKinetic;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character targetCharacter, GridTileController targetTile)
    {
        if (targetCharacter == null) return;

        int initialHealth = targetCharacter.CurrentHealth;
        int maxHealth = targetCharacter.CharacterProfile.TotalVitality;

        targetCharacter.TakeElementalDamage(effect.Amount, effect.Type);
        int currentHealth = targetCharacter.CurrentHealth;

        Debug.Log($"[{targetCharacter.UserFriendlyName}] received [{effect.Amount}] points of damage!\n[{initialHealth}/{maxHealth}] -> [{currentHealth}/{maxHealth}]");
    }
}
