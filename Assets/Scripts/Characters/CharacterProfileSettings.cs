using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfileSettings", menuName = "Settings/CharacterProfileSettings")]
public class CharacterProfileSettings : ScriptableObject
{
    [SerializeField]
    private int _minVitality;
    public int MinVitality
    {
        get => _minVitality;
        set => _minVitality = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxVitality;
    public int MaxVitality
    {
        get => _maxVitality;
        set => _maxVitality = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minPower;
    public int MinPower
    {
        get => _minPower;
        set => _minPower = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxPower;
    public int MaxPower
    {
        get => _maxPower;
        set => _maxPower = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minEndurance;
    public int MinEndurance
    {
        get => _minEndurance;
        set => _minEndurance = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxEndurance;
    public int MaxEndurance
    {
        get => _maxEndurance;
        set => _maxEndurance = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minArmor;
    public int MinArmor
    {
        get => _minArmor;
        set => _minArmor = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxArmor;
    public int MaxArmor
    {
        get => _maxArmor;
        set => _maxArmor = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minAmmo;
    public int MinAmmo
    {
        get => _minAmmo;
        set => _minAmmo = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxAmmo;
    public int MaxAmmo
    {
        get => _maxAmmo;
        set => _maxAmmo = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minInitiative;
    public int MinInitiative
    {
        get => _minInitiative;
        set => _minInitiative = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxInitiative;
    public int MaxInitiative
    {
        get => _maxInitiative;
        set => _maxInitiative = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _minMovementSpeed;
    public int MinMovementSpeed
    {
        get => _minMovementSpeed;
        set => _minMovementSpeed = Mathf.Max(0, value);
    }

    [SerializeField]
    private int _maxMovementSpeed;
    public int MaxMovementSpeed
    {
        get => _maxMovementSpeed;
        set => _maxMovementSpeed = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minAccuracy;
    public float MinAccuracy
    {
        get => _minAccuracy;
        set => _minAccuracy = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxAccuracy;
    public float MaxAccuracy
    {
        get => _maxAccuracy;
        set => _maxAccuracy = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minEvasion;
    public float MinEvasion
    {
        get => _minEvasion;
        set => _minEvasion = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxEvasion;
    public float MaxEvasion
    {
        get => _maxEvasion;
        set => _maxEvasion = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minCritChance;
    public float MinCritChance
    {
        get => _minCritChance;
        set => _minCritChance = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxCritChance;
    public float MaxCritChance
    {
        get => _maxCritChance;
        set => _maxCritChance = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minCritDamage;
    public float MinCritDamage
    {
        get => _minCritDamage;
        set => _minCritDamage = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxCritDamage;
    public float MaxCritDamage
    {
        get => _maxCritDamage;
        set => _maxCritDamage = Mathf.Max(0, value);
    }





    [SerializeField]
    private float _minStunResist;
    public float MinStunResist
    {
        get => _minStunResist;
        set => _minStunResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxStunResist;
    public float MaxStunResist
    {
        get => _maxStunResist;
        set => _maxStunResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minEnergyResist;
    public float MinEnergyResist
    {
        get => _minEnergyResist;
        set => _minEnergyResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxEnergyResist;
    public float MaxEnergyResist
    {
        get => _maxEnergyResist;
        set => _maxEnergyResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minPoisonResist;
    public float MinPoisonResist
    {
        get => _minPoisonResist;
        set => _minPoisonResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxPoisonResist;
    public float MaxPoisonResist
    {
        get => _maxPoisonResist;
        set => _maxPoisonResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minBurnResist;
    public float MinBurnResist
    {
        get => _minBurnResist;
        set => _minBurnResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxBurnResist;
    public float MaxBurnResist
    {
        get => _maxBurnResist;
        set => _maxBurnResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _minBleedResist;
    public float MinBleedResist
    {
        get => _minBleedResist;
        set => _minBleedResist = Mathf.Max(0, value);
    }

    [SerializeField]
    private float _maxBleedResist;
    public float MaxBleedResist
    {
        get => _maxBleedResist;
        set => _maxBleedResist = Mathf.Max(0, value);
    }
}
