using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlasmaHandler : ICombatAbilityEffectHandler
{
    public static CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.DamagePlasma;


    public static void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster)
    {
        List<Character> affected = new List<Character>();

        foreach(var tilePosition in effect.RelativeAffectedPositions)
        {
            GridTile tile = map.GetTile(tilePosition + target);
            if(tile == null) continue;

            foreach(var occupant in tile.Occupants)
            {
                if(occupant is Character) affected.Add((Character) occupant);
            }
        }

        foreach(var character in affected)
        {
            character.TakeElementalDamage(effect.Amount, EffectType);
            if(effect.ForTurns > 0)
            {
                effect.ForTurns--;
                character.AddEffect(effect);
            }
        }
    }

    public static void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        affected.TakeElementalDamage(effect.Amount, EffectType);
        effect.ForTurns--;
    }
}
