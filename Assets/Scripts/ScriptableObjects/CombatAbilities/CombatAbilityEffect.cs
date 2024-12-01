using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAbilityEffect
{
    public EffectType Type;
    public int Amount;

    public List<Vector2Int> RelativeAffectedPositions = new();

    public enum EffectType
    {
        None = 0,
        Damage = 1,
        Heal = 2,
        Stun = 3,
    }

    public static CombatAbilityEffect Duplicate(CombatAbilityEffect original)
    {
        CombatAbilityEffect copy = new CombatAbilityEffect();
        copy.Type = original.Type;
        copy.Amount = original.Amount;
        foreach(var pos in original.RelativeAffectedPositions)
        {
            copy.RelativeAffectedPositions.Add(pos);
        }
        return copy;
    }
}
