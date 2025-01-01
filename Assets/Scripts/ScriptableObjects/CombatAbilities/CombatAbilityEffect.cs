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
        foreach (var pos in original.RelativeAffectedPositions)
        {
            copy.RelativeAffectedPositions.Add(pos);
        }
        return copy;
    }

    private static Dictionary<EffectType, Color> _colors = new()
    {
        { EffectType.Move, new Color(0.1f, 0.3f, 0.3f) },
        { EffectType.SkipTurn, new Color(0.2f, 0.4f, 0.2f) },
        { EffectType.Interact, new Color(0.2f, 0.4f, 0.2f) },
        { EffectType.Reload, new Color(0.2f, 0.4f, 0.2f) },
        { EffectType.DamageKinetic, new Color(0.3f, 0.3f, 0.4f) },
        { EffectType.DamageEnergy, new Color(0.1f, 0.2f, 0.4f) },
        { EffectType.DamageFire, new Color(0.4f, 0.1f, 0.1f) },
        { EffectType.DamagePlasma, new Color(0.3f, 0.0f, 0.4f) },
        { EffectType.DamageAcid, new Color(0.4f, 0.3f, 0.0f) },
        { EffectType.Heal, new Color(0.0f, 0.4f, 0.0f) },
        { EffectType.Shield, new Color(0.0f, 0.4f, 0.4f) },

        { EffectType.None, new Color(0.3f, 0.3f, 0.3f) } //Don't remove
    };
    public static Color EffectTypeToColor(EffectType effectType)
    {
        try
        {
            if(_colors.ContainsKey(effectType)) return _colors[effectType];
            else return _colors[EffectType.None];
        }
        catch (KeyNotFoundException)
        {
            return new Color(0.3f, 0.3f, 0.3f);
        }
    }
}
