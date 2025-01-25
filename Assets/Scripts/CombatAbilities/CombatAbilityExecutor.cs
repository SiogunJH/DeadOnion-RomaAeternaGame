using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VInspector.Libs;

public class CombatAbilityExecutor : MonoBehaviourSingleton<CombatAbilityExecutor>
{
    private Dictionary<CombatAbilityEffect.EffectType, CombatAbilityEffectHandler> _handlers = new()
    {
        {new HealHandler().EffectType , new HealHandler()}
    };

    public delegate void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster);
    private Dictionary<CombatAbilityEffect.EffectType, GridEffectHandler> _gridHandlers = new();

    public delegate void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected);
    private Dictionary<CombatAbilityEffect.EffectType, CharacterEffectHandler> _characterHandlers = new();



    public void ExecuteAbility(Vector2 target, CombatAbility ability, GridMap map, Character caster)
    {
        foreach(var effect in ability.AbilityEffects)
        {
            if (!_gridHandlers.ContainsKey(effect.Type))
            {
                Debug.LogError($"No handler of type: {effect.Type} was found when trying to execute: {ability.name} from: {caster.name}");
                return;
            }
        }

        foreach(var effect in ability.AbilityEffects)
        {
            _gridHandlers[effect.Type]?.Invoke(target, effect, map, caster);
        }
    }
    public void ExecuteEffectOnCharacter(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        _characterHandlers[effect.Type]?.Invoke(effect, map, affected);
    }
}
