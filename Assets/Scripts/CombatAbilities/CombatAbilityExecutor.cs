using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VInspector.Libs;

public class CombatAbilityExecutor : MonoBehaviour
{
    public static CombatAbilityExecutor Instance;

    public delegate void GridEffectHandler(Vector2 target, CombatAbilityEffect effect, GridMap map, Character caster);
    private Dictionary<CombatAbilityEffect.EffectType, GridEffectHandler> _gridHandlers = new();

    public delegate void CharacterEffectHandler(CombatAbilityEffect effect, GridMap map, Character affected);
    private Dictionary<CombatAbilityEffect.EffectType, CharacterEffectHandler> _characterHandlers = new();


    private void Awake()
    {
        Instance = this;
        LoadHandlers();
    }
    private void LoadHandlers() //Loads delegates from all classes implementing ICombatAbilityEffectHandler
    {
        var assembly = typeof(CombatAbilityExecutor).Assembly;
        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            if (!typeof(ICombatAbilityEffectHandler).IsAssignableFrom(type)) continue;

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name.Contains("EffectHandler"));
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name.Contains("EffectType"));

            //There should be two public static methods and one property, unless you add more types of handler methods
            if (methods.Count() > 2 || properties.Count() > 1) Debug.LogWarning($"To many handlers or types found in {type.Name}");

            //Find the right methods and create delegates
            foreach(var method in methods)
            {
                var property = properties.FirstOrDefault();
            
                if(method == null || property == null)
                {
                    Debug.LogError($"Something went wrong when trying to load ability handler: {type.Name}");
                    return;
                }

                if (method.Name.Contains("Grid"))
                {
                    var handler = (GridEffectHandler)System.Delegate.CreateDelegate(typeof(GridEffectHandler), method);
                    CombatAbilityEffect.EffectType effectType = (CombatAbilityEffect.EffectType)property.GetValue(null);
                    _gridHandlers.Add(effectType, handler);
                }
                else if (method.Name.Contains("Character"))
                {
                    var handler = (CharacterEffectHandler)System.Delegate.CreateDelegate(typeof(CharacterEffectHandler), method);
                    CombatAbilityEffect.EffectType effectType = (CombatAbilityEffect.EffectType)property.GetValue(null);
                    _characterHandlers.Add(effectType, handler);
                }
            }
        }
        if (_gridHandlers.Count != _characterHandlers.Count) Debug.LogError("Number of grid effect handlers is not equal to character effect handlers");
    }



    public void ExecuteAbility(Vector2 target, CombatAbility ability, GridMap map, Character caster)
    {
        foreach(var effect in ability.AbilityEffects)
        {
            if (!_gridHandlers.ContainsKey(effect.Type))
            {
                Debug.LogError($"No handler of type: {effect.Type} was found when trying to execute: {ability.name} from: {caster.name}");
                return;
            }
        }

        foreach(var effect in ability.AbilityEffects)
        {
            _gridHandlers[effect.Type]?.Invoke(target, effect, map, caster);
        }
    }
    public void ExecuteEffectOnCharacter(CombatAbilityEffect effect, GridMap map, Character affected)
    {
        _characterHandlers[effect.Type]?.Invoke(effect, map, affected);
    }
}
