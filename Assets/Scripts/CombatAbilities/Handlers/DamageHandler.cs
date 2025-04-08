using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : CombatAbilityEffectHandler
{
    public override CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Damage;

    protected override void DoEffect(CombatAbilityEffect effect, Character caster, Character target, GridTileController targetTile)
    {
        if (target == null) return;

        string log = $"[DAMAGE EFFECT] '{caster.UserFriendlyName}' attacks '{target.UserFriendlyName}'";

        // Calculate hit
        float hitChance = caster.Accuracy - target.Evasion;
        bool isHit = hitChance > 0f && Random.Range(0f, 1f) <= hitChance;

        // On miss
        if (!isHit)
        {
            log += $"\nMissed at {Mathf.RoundToInt(hitChance * 100)}% hit chance!";
            Debug.Log(log);
            return;
        }

        // On hit
        log += $"\nHit at {Mathf.RoundToInt(hitChance * 100)}% hit chance!";


        // Calculate weakspot
        bool isWeakspot = false;
        if (hitChance > 1f)
        {
            float weakspotChance = hitChance - 1;
            isWeakspot = weakspotChance > 0f && Random.Range(0f, 1f) <= weakspotChance;
            log += $"\nWeakspot hit {(isWeakspot ? "succeeded" : "failed")} at {Mathf.RoundToInt(weakspotChance * 100)}% weakspot chance!";
        }

        // Calculate crittical
        bool isCritical;
        {
            float criticalChance = caster.CritChance;
            isCritical = criticalChance > 0f && Random.Range(0f, 1f) <= criticalChance;
            log += $"\nCritical hit {(isCritical ? "succeeded" : "failed")} at {Mathf.RoundToInt(criticalChance * 100)}% critical chance!";
        }

        // Get damage
        int damageRoll = Random.Range(effect.MinMax.x, effect.MinMax.y + 1);
        log += $"\nDamage roll from {effect.MinMax.x} to {effect.MinMax.y} resulted in {damageRoll}!";

        // Get modifier
        float damageModifier = 1f;
        if (isWeakspot) damageModifier += caster.WeakspotDamage;
        if (isCritical) damageModifier += caster.CritDamage;
        log += $"\nDamage modifier is {Mathf.RoundToInt(damageModifier * 100)}% of base damage!";

        // Modified damage
        int modifiedDamage = Mathf.RoundToInt(damageModifier * damageRoll);
        log += $"\nDamage after modification is {modifiedDamage}!";

        // Armor reduction
        int armorReduction = target.CharacterProfile.TotalArmor;
        log += $"\nTarget armor reduction is {armorReduction}!";

        // Final damage
        int finalDamage = modifiedDamage - armorReduction;
        log += $"\nFinal damage is {finalDamage}!";

        Debug.Log(log);
        target.TakeDamage(finalDamage);
    }
}
