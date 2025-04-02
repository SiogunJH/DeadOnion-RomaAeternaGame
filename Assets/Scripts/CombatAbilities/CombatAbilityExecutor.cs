using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatAbilityExecutor
{
    public static HashSet<Character> AffectedCharacters = new();

    private static readonly Dictionary<CombatAbilityEffect.EffectType, CombatAbilityEffectHandler> _handlers = new()
    {
        {new HealHandler().EffectType, new HealHandler()},
        {new MoveHandler().EffectType, new MoveHandler()},
        {new DamageHandler().EffectType, new DamageHandler()},
        {new SkipTurnHandler().EffectType, new SkipTurnHandler()},
    };

    public static void ExecuteAbility(Vector2 target, CombatAbility ability, GridMap map, Character caster)
    {
        Debug.Assert(ability != null, "Ability is null");

        // Debug.Log($"Executing ability: [{ability.Name}]");
        foreach (var effect in ability.AbilityEffects)
        {
            // Debug.Log($"Executing effect: [{effect.Type}]");
            if (_handlers.TryGetValue(effect.Type, out var handler))
            {
                handler.GridEffectHandler(target, effect, map, caster);
            }
            else
            {
                Debug.LogError($"No handler of type: {effect.Type} was found when trying to execute: {ability.name} from: {caster.name}");
            }
        }

        // Play animation if needed
        if (ability.PlaysAnimation)
        {
            AnimationPlayer.Instance.PlayAnimation();
        }
        caster.RemoveActionPoints(CombatManager.Instance.CurrentAbility.ActionPointCost);
        caster.TryToEndTurn();
    }
    public static void ExecuteEffectOnCharacter(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        _handlers[effect.Type].CharacterEffectHandler(effect, map, affected);
    }
}
