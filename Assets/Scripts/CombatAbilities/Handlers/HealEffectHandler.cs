using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffectHandler : ICombatAbilityEffectHandler
{
    public static CombatAbilityEffect.EffectType EffectType => CombatAbilityEffect.EffectType.Heal; //Ties this handler to the EffectType, each EffectType should only have one implementation

    //Executed when an ability is used
    public static void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster)
    {
        List<Character> affected = new List<Character>();

        foreach(var tilePosition in effect.RelativeAffectedPositions) //Check each tile around the target withing the effect range
        {
            GridTile tile = map.GetTile(tilePosition + target); //Check if tiles within range exists on the grid
            if(tile == null) continue;

            foreach(var occupant in tile.Occupants) //Grab all Characters on every affected tile
            {
                if(occupant is Character) affected.Add((Character) occupant);
            }
        }

        foreach(var character in affected)//Execute effect
        {
            character.Heal(effect.Amount); //Execute
            if(effect.ForTurns > 0) //If the effect is done overtime, add to list of acctive effects that will be executed each turn until finished or removed.
            {
                effect.ForTurns--;
                character.AddEffect(effect);
            }
        }
    }
    //Executed each turn by the affected character untill the effect ends
    public static void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        affected.Heal(effect.Amount);
        effect.ForTurns--;
    }
}
