using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatAbilityEffectHandler
{
    public static CombatAbilityEffect.EffectType EffectType { get; }
    public static void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster) { }
    public static void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected) { }
}
