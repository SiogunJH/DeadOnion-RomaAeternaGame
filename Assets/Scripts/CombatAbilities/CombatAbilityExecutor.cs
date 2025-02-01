using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAbilityExecutor : MonoBehaviourSingleton<CombatAbilityExecutor>
{
    private Dictionary<CombatAbilityEffect.EffectType, CombatAbilityEffectHandler> _handlers = new()
    {
        {new HealHandler().EffectType, new HealHandler()},
        {new DamageAcidHandler().EffectType, new DamageAcidHandler()},
        {new DamageEnergyHandler().EffectType, new DamageEnergyHandler()},
        {new DamageFireHandler().EffectType, new DamageFireHandler()},
        {new DamageKineticHandler().EffectType, new DamageKineticHandler()},
        {new DamagePlasmaHandler().EffectType, new DamagePlasmaHandler()},
        {new SkipTurnHandler().EffectType, new SkipTurnHandler()}
    };

    public void ExecuteAbility(Vector2 target, CombatAbility ability, GridMap map, Character caster)
    {
        foreach(var effect in ability.AbilityEffects)
        {
            if (!_handlers.ContainsKey(effect.Type))
            {
                Debug.LogError($"No handler of type: {effect.Type} was found when trying to execute: {ability.name} from: {caster.name}");
                return;
            }
        }

        foreach(var effect in ability.AbilityEffects)
        {
            _handlers[effect.Type].GridEffectHandler(target, effect, map, caster);
        }
    }
    public void ExecuteEffectOnCharacter(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        _handlers[effect.Type].CharacterEffectHandler(effect, map, affected);
    }
}
