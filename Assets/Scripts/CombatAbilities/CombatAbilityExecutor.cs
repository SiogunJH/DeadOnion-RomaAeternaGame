using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatAbilityExecutor
{
    private static readonly Dictionary<CombatAbilityEffect.EffectType, CombatAbilityEffectHandler> _handlers = new()
    {
        {new HealHandler().EffectType, new HealHandler()},
        {new MoveHandler().EffectType, new MoveHandler()},
        {new DamageAcidHandler().EffectType, new DamageAcidHandler()},
        {new DamageEnergyHandler().EffectType, new DamageEnergyHandler()},
        {new DamageFireHandler().EffectType, new DamageFireHandler()},
        {new DamageKineticHandler().EffectType, new DamageKineticHandler()},
        {new DamagePlasmaHandler().EffectType, new DamagePlasmaHandler()},
        {new SkipTurnHandler().EffectType, new SkipTurnHandler()}
    };

    public static void ExecuteAbility(Vector2 target, CombatAbility ability, GridMap map, Character caster)
    {
        Debug.Assert(ability != null, "Ability is null");

        Debug.Log($"Executing ability: [{ability.Name}]");
        foreach (var effect in ability.AbilityEffects)
        {
            Debug.Log($"Executing effect: [{effect.Type}]");
            if (_handlers.TryGetValue(effect.Type, out var handler))
            {
                handler.GridEffectHandler(target, effect, map, caster);
            }
            else
            {
                Debug.LogError($"No handler of type: {effect.Type} was found when trying to execute: {ability.name} from: {caster.name}");
            }
        }
    }
    public static void ExecuteEffectOnCharacter(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        _handlers[effect.Type].CharacterEffectHandler(effect, map, affected);
    }
}
