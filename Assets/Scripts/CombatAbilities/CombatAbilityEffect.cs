using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAbilityEffect
{
    public EffectType Type;
    public int Amount;
    public int ForAdditionalTurns; //For how many turns the effect will last, 0 is executed once, 1 is executed twice (now and next turn)
    public Character.Attribute ChangedAttribute = Character.Attribute.None;

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
        Shield = 11,
        ChangeAttribute = 12
    }

    public static CombatAbilityEffect CloneDeep(CombatAbilityEffect original)
    {
        CombatAbilityEffect copy = new CombatAbilityEffect();
        copy.Type = original.Type;
        copy.Amount = original.Amount;
        copy.ForAdditionalTurns = original.ForAdditionalTurns;
        copy.ChangedAttribute = original.ChangedAttribute;
        foreach (var pos in original.RelativeAffectedPositions)
        {
            copy.RelativeAffectedPositions.Add(new Vector2Int(pos.x, pos.y));
        }
        return copy;
    }
}
