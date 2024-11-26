using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAbilityEffect
{
    public EffectType Type;
    public int Amount;

    public Vector2Int[] RelativeAffectedPositions;

    public enum EffectType
    {
        None = 0,
        Damage = 1,
        Heal = 2,
        Stun = 3,
    }
}
