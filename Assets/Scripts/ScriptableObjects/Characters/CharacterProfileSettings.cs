using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfileSettings", menuName = "Settings/CharacterProfileSettings")]
public class CharacterProfileSettings : ScriptableObject
{
    #region Vitality
    [SerializeField]
    private int _healthPerVitalityPoint;
    public int HealthPerVitalityPoint
    {
        get => _healthPerVitalityPoint;
        set => _healthPerVitalityPoint = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minimalHealthPoints;
    public int MinimalHealthPoints
    {
        get => _minimalHealthPoints;
        set => _minimalHealthPoints = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _endurancePercentsPerVitalityPoint;
    public float EndurancePercentsPerVitalityPoint
    {
        get => _endurancePercentsPerVitalityPoint;
        set => _endurancePercentsPerVitalityPoint = Mathf.Max(0f, value);
    }

    [SerializeField]
    private float _minimalEndurancePercents;
    public float MinimalEndurancePercents
    {
        get => _minimalEndurancePercents;
        set => _minimalEndurancePercents = Mathf.Max(0f, value);
    }
    #endregion

    #region Strength
    [SerializeField]
    private int _weaponDamagePerStrengthPoint;
    public int WeaponDamagePerStrengthPoint
    {
        get => _weaponDamagePerStrengthPoint;
        set => _weaponDamagePerStrengthPoint = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minimalWeaponDamage;
    public int MinimalWeaponDamage
    {
        get => _minimalWeaponDamage;
        set => _minimalWeaponDamage = Mathf.Max(0, value);
    }
    #endregion

    #region Power
    [SerializeField]
    private int _abilityDamagePerPowerPoint;
    public int AbilityDamagePerPowerPoint
    {
        get => _abilityDamagePerPowerPoint;
        set => _abilityDamagePerPowerPoint = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minimalAbilityDamage;
    public int MinimalAbilityDamage
    {
        get => _minimalAbilityDamage;
        set => _minimalAbilityDamage = Mathf.Max(0, value);
    }
    #endregion

    #region Agility
    [SerializeField]
    private int _evasionPerAgilityPoint;
    public int EvasionPerAgilityPoint
    {
        get => _evasionPerAgilityPoint;
        set => _evasionPerAgilityPoint = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minimalEvasion;
    public int MinimalEvasion
    {
        get => _minimalEvasion;
        set => _minimalEvasion = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _agilityPointsPerMoveSpeed = 1;
    public int AgilityPointsPerMoveSpeed
    {
        get => _agilityPointsPerMoveSpeed;
        set => _agilityPointsPerMoveSpeed = Mathf.Max(1, value);
    }

    [SerializeField]
    private int _minimalMoveSpeed;
    public int MinimalMoveSpeed
    {
        get => _minimalMoveSpeed;
        set => _minimalMoveSpeed = Mathf.Max(0, value);
    }
    #endregion

    #region Focus
    [SerializeField]
    private int _accuracyPerFocusPoints;
    public int AccuracyPerFocusPoints
    {
        get => _accuracyPerFocusPoints;
        set => _accuracyPerFocusPoints = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minimalAccuracy;
    public int MinimalAccuracy
    {
        get => _minimalAccuracy;
        set => _minimalAccuracy = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _critDamagePercentsPerFocusPoint;
    public float CritDamagePercentsPerFocusPoint
    {
        get => _critDamagePercentsPerFocusPoint;
        set => _critDamagePercentsPerFocusPoint = Mathf.Max(0f, value);
    }

    [SerializeField]
    private float _minimalCritDamagePercents;
    public float MinimalCritDamagePercents
    {
        get => _minimalCritDamagePercents;
        set => _minimalCritDamagePercents = Mathf.Max(0f, value);
    }
    #endregion

    #region Reflex
    [SerializeField]
    private int _initiativePerReflexPoint;
    public int InitiativePerReflexPoint
    {
        get => _initiativePerReflexPoint;
        set => _initiativePerReflexPoint = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minimalInitiative;
    public int MinimalInitiative
    {
        get => _minimalInitiative;
        set => _minimalInitiative = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _critChancePercentsPerReflexPoint;
    public float CritChancePercentsPerReflexPoint
    {
        get => _critChancePercentsPerReflexPoint;
        set => _critChancePercentsPerReflexPoint = Mathf.Max(0f, value);
    }

    [SerializeField]
    private float _minimalCritChancePercents;
    public float MinimalCritChancePercents
    {
        get => _minimalCritChancePercents;
        set => _minimalCritChancePercents = Mathf.Max(0f, value);
    }
    #endregion
}