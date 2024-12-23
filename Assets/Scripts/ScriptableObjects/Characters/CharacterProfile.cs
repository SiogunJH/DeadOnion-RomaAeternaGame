using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//damage over time, skip/add turn, change stats, add action point
public class CharacterProfile : ScriptableObject
{
    public CharacterProfileSettings Settings { get; private set; }
    private void OnEnable()
    {
        if (Settings == null)
        {
            var set = Resources.LoadAll<CharacterProfileSettings>("Settings");
            if (set.Length > 1) Debug.LogError("More than one CharacterProfileSettings asset found");
            if(set.Length <= 0)
            {
                Debug.LogError("No CharacterProfileSettings asset found");
                return;
            }
            Settings = set.FirstOrDefault();
        }
    }



    public string Name = "NOT SET";


    #region >>> Primary Attributes <<<

    [SerializeField] [HideInInspector]
    private int _vitality = 1;
    public int Vitality
    {
        get => _vitality;
        set => _vitality = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _strength = 1;
    public int Strength
    {
        get => _strength;
        set => _strength = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _power = 1;
    public int Power
    {
        get => _power;
        set => _power = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _agility = 1;
    public int Agility
    {
        get => _agility;
        set => _agility = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _focus = 1;
    public int Focus
    {
        get => _focus;
        set => _focus = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _reflex = 1;
    public int Reflex
    {
        get => _reflex;
        set => _reflex = Mathf.Max(1, value);
    }

    #endregion


    #region >>> Secondary Attributes <<<

    public int BaseHealth => Mathf.Max(Vitality * Settings.HealthPerVitalityPoint, Settings.MinimalHealthPoints);
    [SerializeField] [HideInInspector]
    private int _additionalHealth;
    public int AdditionalHealth
    {
        get => _additionalHealth;
        set => _additionalHealth = Mathf.Max(0, value);
    }
    public int TotalHealth => Mathf.Min(BaseHealth + AdditionalHealth, MaxHealth);

    public float BaseEndurancePercentage => Mathf.Max(Vitality * Settings.EndurancePercentsPerVitalityPoint, Settings.MinimalEndurancePercents);
    [SerializeField] [HideInInspector]
    private float _additionalEndurancePercentage;
    public float AdditionalEndurancePercentage
    {
        get => _additionalEndurancePercentage;
        set => _additionalEndurancePercentage = Mathf.Max(0, value);
    }
    public float TotalEndurance => BaseEndurancePercentage + AdditionalEndurancePercentage;


    public int BaseWeaponDamage => Mathf.Max(Strength * Settings.WeaponDamagePerStrengthPoint, Settings.MinimalWeaponDamage);
    [SerializeField] [HideInInspector]
    private int _additionalWeaponDamage;
    public int AdditionalWeaponDamage
    {
        get => _additionalWeaponDamage;
        set => _additionalWeaponDamage = Mathf.Max(0, value);
    }
    public int TotalWeaponDamage => BaseWeaponDamage + AdditionalWeaponDamage;


    public int BaseAbilityDamage => Mathf.Max(Power * Settings.AbilityDamagePerPowerPoint, Settings.MinimalAbilityDamage);
    [SerializeField] [HideInInspector]
    private int _additionalAbilityDamage;
    public int AdditionalAbilityDamage
    {
        get => _additionalAbilityDamage;
        set => _additionalAbilityDamage = Mathf.Max(0, value);
    }
    public int TotalAbilityDamage => BaseAbilityDamage + AdditionalAbilityDamage;


    public int BaseEvasion => Mathf.Max(Agility * Settings.EvasionPerAgilityPoint, Settings.MinimalEvasion);
    [SerializeField] [HideInInspector]
    private int _additionalEvasion;
    public int AdditionalEvasion
    {
        get => _additionalEvasion;
        set => _additionalEvasion = Mathf.Max(0, value);
    }
    public int TotalEvasion => BaseEvasion + AdditionalEvasion;

    public int BaseMoveSpeed => Mathf.Max(Agility / Settings.AgilityPointsPerMoveSpeed, Settings.MinimalMoveSpeed);
    [SerializeField] [HideInInspector]
    private int _additionalMoveSpeed;
    public int AdditionalMoveSpeed
    {
        get => _additionalMoveSpeed;
        set => _additionalMoveSpeed = Mathf.Max(0, value);
    }
    public int TotalMoveSpeed => BaseMoveSpeed + AdditionalMoveSpeed;


    public int BaseAccuracy => Mathf.Max(Focus * Settings.AccuracyPerFocusPoints, Settings.MinimalAccuracy);
    [SerializeField] [HideInInspector]
    private int _additionalAccuracy;
    public int AdditionalAccuracy
    {
        get => _additionalAccuracy;
        set => _additionalAccuracy = Mathf.Max(0, value);
    }
    public int TotalAccuracy => BaseAccuracy + AdditionalAccuracy;

    public float BaseCritDamagePercentage => Mathf.Max(Focus * Settings.CritDamagePercentsPerFocusPoint, Settings.MinimalCritDamagePercents);
    [SerializeField] [HideInInspector]
    private float _additionalCritDamagePercentage;
    public float AdditionalCritDamagePercentage
    {
        get => _additionalCritDamagePercentage;
        set => _additionalCritDamagePercentage = Mathf.Max(0, value);
    }
    public float TotalCritDamagePercentage => BaseCritDamagePercentage + AdditionalCritDamagePercentage;


    public int BaseInitiative => Mathf.Max(Reflex * Settings.InitiativePerReflexPoint, Settings.MinimalInitiative);
    [SerializeField] [HideInInspector]
    private int _additionalInitiative;
    public int AdditionalInitiative
    {
        get => _additionalInitiative;
        set => _additionalInitiative = Mathf.Max(0, value);
    }
    public int TotalInitiative => BaseInitiative + AdditionalInitiative;

    public float BaseCritChancePercentage => Mathf.Max(Reflex * Settings.CritChancePercentsPerReflexPoint, Settings.MinimalCritChancePercents);
    [SerializeField] [HideInInspector]
    private float _additionalCritChancePercentage;
    public float AdditionalCritChancePercentage
    {
        get => _additionalCritChancePercentage;
        set => _additionalCritChancePercentage = Mathf.Max(0, value);
    }
    public float TotalCritChancePercentage => BaseCritChancePercentage + AdditionalCritChancePercentage;

    #endregion


    #region >>> Tertiary Attributes <<<

    [SerializeField] [HideInInspector]
    private int _actionPoints = 4;
    public int ActionPoints
    {
        get => _actionPoints;
        set => _actionPoints = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _maxArmor = 99999;
    public int MaxArmor
    {
        get => _maxArmor;
        set => _maxArmor = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _maxHealth = 99999;
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    private int _maxShield = 99999;
    public int MaxShield
    {
        get => _maxShield;
        set => _maxShield = Mathf.Max(1, value);
    }

    [SerializeField] [HideInInspector]
    public CharacterArmorClass ArmorClass;


    public enum CharacterArmorClass
    {
        Light = 0,
        Medium = 1,
        Heavy = 2
    }

    #endregion


    [HideInInspector]
    public List<CombatAbility> CombatAbilities = new();
}
