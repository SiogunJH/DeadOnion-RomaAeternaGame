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
        foreach(var pos in original.RelativeAffectedPositions)
        {
            copy.RelativeAffectedPositions.Add(pos);
        }
        return copy;
    }
    public static Color EffectTypeToColor(EffectType effectType)
    {
        switch(effectType) //try to keep colors under 0.5f
        {
            case EffectType.Move:
                return new Color(0.1f, 0.3f, 0.3f);

            case EffectType.SkipTurn:
                return new Color(0.2f, 0.4f, 0.2f);

            case EffectType.Interact:
                return new Color(0.2f, 0.4f, 0.2f);

            case EffectType.Reload:
                return new Color(0.2f, 0.4f, 0.2f);

            case EffectType.DamageKinetic:
                return new Color(0.3f, 0.3f, 0.4f);

            case EffectType.DamageEnergy:
                return new Color(0.1f, 0.2f, 0.4f);

            case EffectType.DamageFire:
                return new Color(0.4f, 0.1f, 0.1f);

            case EffectType.DamagePlasma:
                return new Color(0.3f, 0.0f, 0.4f);

            case EffectType.DamageAcid:
                return new Color(0.4f, 0.3f, 0.0f);

            case EffectType.Heal:
                return new Color(0.0f, 0.4f, 0.0f);

            case EffectType.Shield:
                return new Color(0.0f, 0.4f, 0.4f);

            case EffectType.None:
            default:
                return new Color(0.3f, 0.3f, 0.3f);  
        }
    }
}
