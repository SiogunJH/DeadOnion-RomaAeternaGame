using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAbilityEffect
{
    public EffectType Type;
    public int Amount;
    public Vector2Int MinMax;
    public int ForAdditionalTurns; //For how many turns the effect will last, 0 is executed once, 1 is executed twice (now and next turn)
    public Character.Attribute ChangedAttribute = Character.Attribute.None;

    public List<Vector2Int> RelativeAffectedPositions = new();

    public const int MINMAX_FIELD_FLAG = 1 << 12;
    public const int DOT_FIELD_FLAG = 1 << 13;
    public const int AMOUNT_FIELD_FLAG = 1 << 14;
    public const int ATTRIBUTE_FIELD_FLAG = 1 << 15;

    public enum EffectType
    {
        None = 0,

        Move = 1,
        SkipTurn = 2,
        Interact = 3,
        Reload = 4,

        Damage = 5 + MINMAX_FIELD_FLAG,
        Heal = 10 + MINMAX_FIELD_FLAG,
        Shield = 11 + MINMAX_FIELD_FLAG,

        // Other
        ChangeAttribute = 12 + AMOUNT_FIELD_FLAG + ATTRIBUTE_FIELD_FLAG,
    }

    public static CombatAbilityEffect CloneDeep(CombatAbilityEffect original)
    {
        CombatAbilityEffect copy = new()
        {
            Type = original.Type,
            Amount = original.Amount,
            MinMax = new(original.MinMax.x, original.MinMax.y),
            ForAdditionalTurns = original.ForAdditionalTurns,
            ChangedAttribute = original.ChangedAttribute
        };
        foreach (var pos in original.RelativeAffectedPositions)
        {
            copy.RelativeAffectedPositions.Add(new Vector2Int(pos.x, pos.y));
        }
        return copy;
    }
}
