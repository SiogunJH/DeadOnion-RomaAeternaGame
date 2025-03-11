using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//damage over time, skip/add turn, change stats, add action point
public class CharacterProfile : ScriptableObject
{
    [SerializeField, HideInInspector] private CharacterProfileSettings _settings = null;
    public CharacterProfileSettings Settings { get => _settings; }
    public void SetCharacterProfileSettings(CharacterProfileSettings settings)
    {
        if (settings == null) return;
        _settings = settings;
    }

    public void SetDefaultValues()
    {
        _vitality = Settings.MinVitality;
        _power = Settings.MinPower;
        _endurance = Settings.MinEndurance;
        _armor = Settings.MinArmor;
        _ammo = Settings.MinAmmo;
        _initiative = Settings.MinInitiative;
        _movementSpeed = Settings.MinMovementSpeed;
        _accuracy = Settings.MinAccuracy;
        _evasion = Settings.MinEvasion;
        _critChance = Settings.MinCritChance;
        _critDamage = Settings.MinCritDamage;
        _stunResist = Settings.MinStunResist;
        _energyResist = Settings.MinEnergyResist;
        _poisonResist = Settings.MinPoisonResist;
        _burnResist = Settings.MinBurnResist;
        _bleedResist = Settings.MinBleedResist;
    }


    public string Name = "NOT SET";

    #region >>> Primary Attributes <<<

    #region > Stats <

    [SerializeField] [HideInInspector]
    private int _vitality = 1;
    public int BaseVitality
    {
        get { return _vitality; }
        set { _vitality = Mathf.Max(Settings.MinVitality, Mathf.Min(Settings.MaxVitality, value)); }
    }

    [SerializeField] [HideInInspector]
    private int _power = 1;
    public int BasePower
    {
        get { return _power; }
        set { _power = Mathf.Max(Settings.MinPower, Mathf.Min(Settings.MaxPower, value)); }
    }

    [SerializeField] [HideInInspector]
    private int _endurance = 1;
    public int BaseEndurance
    {
        get { return _endurance; }
        set { _endurance = Mathf.Max(Settings.MinEndurance, Mathf.Min(Settings.MaxEndurance, value)); }
    }

    [SerializeField] [HideInInspector]
    private int _armor = 1;
    public int BaseArmor
    {
        get { return _armor; }
        set { _armor = Mathf.Max(Settings.MinArmor, Mathf.Min(Settings.MaxArmor, value)); }
    }

    [SerializeField] [HideInInspector]
    private int _ammo = 1;
    public int BaseAmmo
    {
        get { return _ammo; }
        set { _ammo = Mathf.Max(Settings.MinAmmo, Mathf.Min(Settings.MaxAmmo, value)); }
    }

    [SerializeField] [HideInInspector]
    private int _initiative = 1;
    public int BaseInitiative
    {
        get { return _initiative; }
        set { _initiative = Mathf.Max(Settings.MinInitiative, Mathf.Min(Settings.MaxInitiative, value)); }
    }

    [SerializeField] [HideInInspector]
    private int _movementSpeed = 1;
    public int BaseMovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = Mathf.Max(Settings.MinMovementSpeed, Mathf.Min(Settings.MaxMovementSpeed, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _accuracy = 1f;
    public float BaseAccuracy
    {
        get { return _accuracy; }
        set { _accuracy = Mathf.Max(Settings.MinAccuracy, Mathf.Min(Settings.MaxAccuracy, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _evasion = 1f;
    public float BaseEvasion
    {
        get { return _evasion; }
        set { _evasion = Mathf.Max(Settings.MinEvasion, Mathf.Min(Settings.MaxEvasion, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _critChance = 1f;
    public float BaseCritChance
    {
        get { return _critChance; }
        set { _critChance = Mathf.Max(Settings.MinCritChance, Mathf.Min(Settings.MaxCritChance, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _critDamage = 1f;
    public float BaseCritDamage
    {
        get { return _critDamage; }
        set { _critDamage = Mathf.Max(Settings.MinCritDamage, Mathf.Min(Settings.MaxCritDamage, value)); }
    }

    #endregion


    #region > Resist <

    [SerializeField] [HideInInspector]
    private float _stunResist = 1f;
    public float BaseStunResist
    {
        get { return _stunResist; }
        set { _stunResist = Mathf.Max(Settings.MinStunResist, Mathf.Min(Settings.MaxStunResist, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _energyResist = 1f;
    public float BaseEnergyResist
    {
        get { return _energyResist; }
        set { _energyResist = Mathf.Max(Settings.MinEnergyResist, Mathf.Min(Settings.MaxEnergyResist, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _poisonResist = 1f;
    public float BasePoisonResist
    {
        get { return _poisonResist; }
        set { _poisonResist = Mathf.Max(Settings.MinPoisonResist, Mathf.Min(Settings.MaxPoisonResist, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _burnResist = 1f;
    public float BaseBurnResist
    {
        get { return _burnResist; }
        set { _burnResist = Mathf.Max(Settings.MinBurnResist, Mathf.Min(Settings.MaxBurnResist, value)); }
    }

    [SerializeField] [HideInInspector]
    private float _bleedResist = 1f;
    public float BaseBleedResist
    {
        get { return _bleedResist; }
        set { _bleedResist = Mathf.Max(Settings.MinBleedResist, Mathf.Min(Settings.MaxBleedResist, value)); }
    }

    #endregion

    #endregion


    #region >>> Modified Attributes <<<

    [SerializeField] [HideInInspector]
    private int _additionalVitality = 1;
    public int AdditionalVitality
    {
        get { return _additionalVitality; }
        set { _additionalVitality = value; }
    }
    public int TotalVitality => BaseVitality + AdditionalVitality;

    [SerializeField] [HideInInspector]
    private int _additionalPower = 1;
    public int AdditionalPower
    {
        get { return _additionalPower; }
        set { _additionalPower = value; }
    }
    public int TotalPower => BasePower + AdditionalPower;

    [SerializeField] [HideInInspector]
    private int _additionalEndurance = 1;
    public int AdditionalEndurance
    {
        get { return _additionalEndurance; }
        set { _additionalEndurance = value; }
    }
    public int TotalEndurance => BaseEndurance + AdditionalEndurance;

    [SerializeField] [HideInInspector]
    private int _additionalArmor = 1;
    public int AdditionalArmor
    {
        get { return _additionalArmor; }
        set { _additionalArmor = value; }
    }
    public int TotalArmor => BaseArmor + AdditionalArmor;

    [SerializeField] [HideInInspector]
    private int _additionalAmmo = 1;
    public int AdditionalAmmo
    {
        get { return _additionalAmmo; }
        set { _additionalAmmo = value; }
    }
    public int TotalAmmo => BaseAmmo + AdditionalAmmo;

    [SerializeField] [HideInInspector]
    private int _additionalInitiative = 1;
    public int AdditionalInitiative
    {
        get { return _additionalInitiative; }
        set { _additionalInitiative = value; }
    }
    public int TotalInitiative => BaseInitiative + AdditionalInitiative;

    [SerializeField] [HideInInspector]
    private int _additionalMovementSpeed = 1;
    public int AdditionalMovementSpeed
    {
        get { return _additionalMovementSpeed; }
        set { _additionalMovementSpeed = value; }
    }
    public int TotalMovementSpeed => BaseMovementSpeed + AdditionalMovementSpeed;

    [SerializeField] [HideInInspector]
    private float _additionalAccuracy = 1f;
    public float AdditionalAccuracy
    {
        get { return _additionalAccuracy; }
        set { _additionalAccuracy = value; }
    }
    public float TotalAccuracy => BaseAccuracy + AdditionalAccuracy;

    [SerializeField] [HideInInspector]
    private float _additionalEvasion = 1f;
    public float AdditionalEvasion
    {
        get { return _additionalEvasion; }
        set { _additionalEvasion = value; }
    }
    public float TotalEvasion => BaseEvasion + AdditionalEvasion;

    [SerializeField] [HideInInspector]
    private float _additionalCritChance = 1f;
    public float AdditionalCritChance
    {
        get { return _additionalCritChance; }
        set { _additionalCritChance = value; }
    }
    public float TotalCritChance => BaseCritChance + AdditionalCritChance;

    [SerializeField] [HideInInspector]
    private float _additionalCritDamage = 1f;
    public float AdditionalCritDamage
    {
        get { return _additionalCritDamage; }
        set { _additionalCritDamage = value; }
    }
    public float TotalCritDamage => BaseCritDamage + AdditionalCritDamage;

    [SerializeField] [HideInInspector]
    private float _additionalStunResist = 1f;
    public float AdditionalStunResist
    {
        get { return _additionalStunResist; }
        set { _additionalStunResist = value; }
    }
    public float TotalStunResist => BaseStunResist + AdditionalStunResist;

    [SerializeField] [HideInInspector]
    private float _additionalEnergyResist = 1f;
    public float AdditionalEnergyResist
    {
        get { return _additionalEnergyResist; }
        set { _additionalEnergyResist = value; }
    }
    public float TotalEnergyResist => BaseEnergyResist + AdditionalEnergyResist;

    [SerializeField] [HideInInspector]
    private float _additionalPoisonResist = 1f;
    public float AdditionalPoisonResist
    {
        get { return _additionalPoisonResist; }
        set { _additionalPoisonResist = value; }
    }
    public float TotalPoisonResist => BasePoisonResist + AdditionalPoisonResist;

    [SerializeField] [HideInInspector]
    private float _additionalBurnResist = 1f;
    public float AdditionalBurnResist
    {
        get { return _additionalBurnResist; }
        set { _additionalBurnResist = value; }
    }
    public float TotalBurnResist => BaseBurnResist + AdditionalBurnResist;

    [SerializeField] [HideInInspector]
    private float _additionalBleedResist = 1f;
    public float AdditionalBleedResist
    {
        get { return _additionalBleedResist; }
        set { _additionalBleedResist = value; }
    }
    public float TotalBleedResist => BaseBleedResist + AdditionalBleedResist;

    #endregion




    [HideInInspector]
    public List<CombatAbility> CombatAbilities = new();
}
