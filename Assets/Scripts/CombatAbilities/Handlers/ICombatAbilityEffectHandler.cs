using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Look for HealEffectHandler for example of use
public interface ICombatAbilityEffectHandler
{
    /// <summary>
    /// Handler identifier used by the Executor
    /// </summary>
    public static CombatAbilityEffect.EffectType EffectType { get; }

    /// <summary>
    /// Execute combat ability effects when using abilities on the grid map
    /// </summary>
    /// <param name="target">Absolute coordinate of the tile where the attack was used</param>
    /// <param name="effect">Effect to execute</param>
    /// <param name="map">Reference to the current grid map</param>
    /// <param name="caster">Reference to the person causing the effect, used for ignoring certain targets when ability is used</param>
    public static void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster) { }

    /// <summary>
    /// Execute combat ability effect, on a character already affected by over time effect
    /// </summary>
    /// <param name="effect">Effect to execute</param>
    /// <param name="map">Reference to the current gridmap</param>
    /// <param name="affected">Reference to the character already affected by an over time effect</param>
    public static void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected) { }
}
