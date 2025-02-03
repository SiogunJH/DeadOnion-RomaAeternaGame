using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : GridEntity
{
    [SerializeField]
    private CharacterProfile _profile;
    public CharacterProfile CharacterProfile => _profile;

    private int _currentHealth;
    private int _currentShield = 0;
    private int _currentArmor;

    private int _currentMovePoints;


    private void Awake()
    {
        _currentHealth = CharacterProfile.TotalHealth;
        _currentArmor = CharacterProfile.MaxArmor;
    }


    #region >>> Effect <<<

    private List<CombatAbilityEffect> _activeEffects;

    public void AddEffect(CombatAbilityEffect effect)
    {
        CombatAbilityEffect newEffect = CombatAbilityEffect.CloneDeep(effect);
        _activeEffects.Add(newEffect);
    }
    private void ExecuteActiveEffects()
    {
        _activeEffects = _activeEffects.Where(e => e.ForTurns >= 0).ToList();
        foreach(var effect in _activeEffects)
        {
            CombatAbilityExecutor.ExecuteEffectOnCharacter(effect, GridManager.Instance.Grid, this);
            Debug.Log("Replace null with gridmap reference");
            if(effect.ForTurns <= 0) _activeEffects.Remove(effect);
        }
    }

    public void SkipTurn()
    {
        _hadTurn = true;
    }
    public void Heal(int amount)
    {
        if(amount < 0) return;
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, CharacterProfile.TotalHealth);
    }
    public void TakeTrueDamage(int damage)
    {
        if(damage < 0) return;
        uint maxDamageBlocked = (uint)Mathf.RoundToInt( damage * ArmorClassToDamageReduction(CharacterProfile.ArmorClass));
        uint damageToHealth = (uint)damage - maxDamageBlocked;
        _currentHealth -= (int)damageToHealth;
        _currentArmor -= (int)maxDamageBlocked;
        if(_currentArmor < 0)
        {
            _currentHealth += _currentArmor;
            _currentArmor = 0;
        }
        Die();
    }
    public void TakeElementalDamage(int amount, CombatAbilityEffect.EffectType damageType)
    {
        TakeTrueDamage(amount);
        Debug.LogWarning("Elemental damage not yet implemented");
        return;

        #pragma warning disable CS0162 // Unreachable code detected
        #pragma warning disable IDE0035 // Unreachable code detected
        if (amount < 0) return;
        switch (damageType)
        {
            case CombatAbilityEffect.EffectType.DamageAcid:
            case CombatAbilityEffect.EffectType.DamageKinetic:
            case CombatAbilityEffect.EffectType.DamageEnergy:
            case CombatAbilityEffect.EffectType.DamagePlasma:
            case CombatAbilityEffect.EffectType.DamageFire:
                break;

            default:
                Debug.Log("Given EffectType is not a type of damage");
                return;
        }

        Debug.Log("Do damage here"); //don't forget about armor
        //die
        #pragma warning restore CS0162 // Unreachable code detected
        #pragma warning restore IDE0035 // Unreachable code detected
    }
    public void GainArmor(int amount)
    {
        if(amount < 0) return;
        _currentArmor += amount;
        _currentHealth = Mathf.Min(_currentArmor, CharacterProfile.MaxArmor);
    }
    public void GainShield(int amount)
    {
        if(amount < 0) return;
        _currentShield += amount;
        _currentShield = Mathf.Min(_currentShield, CharacterProfile.MaxShield);
    }
    public void Reload()
    {
        Debug.Log("Character reloaded");
    }
    public void Move()
    {
        Debug.Log("Character Moved");
    }
    public void Interract()
    {
        Debug.Log("Character Interracted");
    }

    #endregion


    #region >>> Attributes <<<

    public enum Attribute //Enum to have a neat attribute name dropdown in tool and avoid using reflection or writing a 100 methods
    {
        None = 0,
        Strength = 1,
        Dex = 2,
        Evasion = 3
    }
    public void ChangeAttribute(Attribute attribute, int value) //called by handlers
    {
        switch(attribute) //Switch for now but it would be messy with all attributes added, maybe dict with methods would be better?
        {
            case Attribute.Evasion:
                _evasionModification += value;
                break;
            //case...
            //case...

            default:
                break;
        }
    }
    private int _evasionModification;
    public int Evasion => _profile.TotalEvasion + _evasionModification;

    //To clear the stat modification:
    //Add some sort of OnDeath to handlers done when overtime effect expires.
    //Or maybe clear and calculate the stat modification each turn????

    //
    //Either do all this or just write a method for each attribute or something like that

    #endregion


    #region >>> Turn <<<

    [SerializeField][HideInInspector]
    private bool _hadTurn = false;
    public bool HadTurn => _hadTurn;


    public void BeginTurn()
    {
        ExecuteActiveEffects();
        //Do turn here
        EndTurn();
    }
    private void EndTurn()
    {
        _hadTurn = true;
        TurnManager.Instance.ContinueTurn();
    }

    public void ResetTurn()
    {
        _hadTurn = false;
        _currentMovePoints = CharacterProfile.TotalMoveSpeed;
    }

    #endregion


    private void Die()
    {
        if (_currentHealth > 0)
            return;
        TurnManager.Instance.RemoveCharacter(this);
        Debug.Log("Character died");
    }



    private readonly static Dictionary<CharacterProfile.CharacterArmorClass, float> _damageReduction = new()
    {
        { CharacterProfile.CharacterArmorClass.Light, 0.4f },
        { CharacterProfile.CharacterArmorClass.Medium, 0.6f },
        { CharacterProfile.CharacterArmorClass.Heavy, 0.8f }
    };
    public static float ArmorClassToDamageReduction(CharacterProfile.CharacterArmorClass armorClass)
    {
        return _damageReduction[armorClass];
    }
}
