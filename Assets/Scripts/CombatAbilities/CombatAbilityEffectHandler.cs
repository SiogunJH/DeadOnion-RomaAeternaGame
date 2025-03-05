using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAbilityEffectHandler
{
    /// <summary>
    /// Handler identifier used by the Executor
    /// </summary>
    public abstract CombatAbilityEffect.EffectType EffectType { get; }

    /// <summary>
    /// Execute combat ability effects when using abilities on the grid map
    /// </summary>
    /// <param name="target">Absolute coordinate of the tile where the attack was used</param>
    /// <param name="effect">Effect to execute</param>
    /// <param name="map">Reference to the current grid map</param>
    /// <param name="caster">Reference to the person causing the effect, used for ignoring certain targets when ability is used</param>
    public virtual void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster)
    {
        List<Character> affected = FindAffectedTargets(target, effect, map, caster);

        foreach (var character in affected)
        {
            DoEffect(effect, character);
            if (effect.ForAdditionalTurns > 0)
            {
                character.AddEffect(effect);
            }
        }

        caster.RemoveActionPoints(CombatManager.Instance.CurrentAbility.ActionPointCost);
        caster.TryToEndTurn();
    }

    /// <summary>
    /// Execute combat ability effect, on a character already affected by over time effect
    /// </summary>
    /// <param name="effect">Effect to execute</param>
    /// <param name="map">Reference to the current gridmap</param>
    /// <param name="affected">Reference to the character already affected by an over time effect</param>
    public virtual void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        DoEffect(effect, affected);
        effect.ForAdditionalTurns--;
    }


    /// <summary>
    /// Override this if you need to change how affected targets are selected
    /// </summary>
    protected virtual List<Character> FindAffectedTargets(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster)
    {
        List<Character> affected = new();
        foreach (var tilePosition in effect.RelativeAffectedPositions)
        {
            GridTileData tile = map[tilePosition.x + (int)target.x, tilePosition.y + (int)target.y];
            if (tile == null) continue;

            foreach (var occupant in tile.Occupants)
            {
                if (occupant is Character) affected.Add((Character)occupant);
            }
        }
        return affected; //As of this moment, doesen't exclude allies
    }

    /// <summary>
    /// Calls each method from Character that this effect is supposed to trigger
    /// </summary>
    protected abstract void DoEffect(CombatAbilityEffect effect, Character affectedCharacter);

}
