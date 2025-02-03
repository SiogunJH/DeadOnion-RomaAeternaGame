using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAbilityEffect
{
    public EffectType Type;
    public int Amount;
    public int ForTurns; //For how many turns the effect will last, 0 is executed once, 1 is executed twice (now and next turn)

    public List<Vector2Int> RelativeAffectedPositions = new();

    public enum EffectType
    {
        None = 0,
        Move = 1,
        SkipTurn = 2,
        Interact = 3,
        Reload = 4,
        DamageKinetic = 5,
        DamageEnergy = 6,
        DamageFire = 7,
        DamagePlasma = 8,
        DamageAcid = 9,
        Heal = 10,
        Shield = 11
    }

    public static CombatAbilityEffect CloneDeep(CombatAbilityEffect original)
    {
        CombatAbilityEffect copy = new CombatAbilityEffect();
        copy.Type = original.Type;
        copy.Amount = original.Amount;
        copy.ForTurns = original.ForTurns;
        foreach (var pos in original.RelativeAffectedPositions)
        {
            copy.RelativeAffectedPositions.Add(new Vector2Int(pos.x, pos.y));
        }
        return copy;
    }
}
