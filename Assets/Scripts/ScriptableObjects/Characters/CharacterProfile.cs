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

    //Primary Attributes
    public int Vitality { get; set; } = 1;
    public int Strength { get; set; } = 1;
    public int Power { get; set; } = 1;
    public int Agility { get; set; } = 1;
    public int Focus { get; set; } = 1;
    public int Reflex { get; set; } = 1;


    #region >>> Secondary Attributes <<<

    public int BaseHealth => Mathf.Max(Vitality * Settings.HealthPerVitalityPoint, Settings.MinimalHealthPoints);
    public int AdditionalHealth { get; set; }
    public int TotalHealth => Mathf.Min(BaseHealth + AdditionalHealth, MaxHealth);
    public float BaseEndurancePercentage => Mathf.Max(Vitality * Settings.EndurancePercentsPerVitalityPoint, Settings.MinimalEndurancePercents);
    public float AdditionalEndurancePercentage { get; set; }
    public float TotalEndurance => BaseEndurancePercentage + AdditionalEndurancePercentage;

    public int BaseWeaponDamage => Mathf.Max(Strength * Settings.WeaponDamagePerStrengthPoint, Settings.MinimalWeaponDamage);
    public int AdditionalWeaponDamage { get; set; }
    public int TotalWeaponDamage => BaseWeaponDamage + AdditionalWeaponDamage;

    public int BaseAbilityDamage => Mathf.Max(Power * Settings.AbilityDamagePerPowerPoint, Settings.MinimalAbilityDamage);
    public int AdditionalAbilityDamage { get; set; }
    public int TotalAbilityDamage => BaseAbilityDamage + AdditionalAbilityDamage;

    public int BaseEvasion => Mathf.Max(Agility * Settings.EvasionPerAgilityPoint, Settings.MinimalEvasion);
    public int AdditionalEvasion { get; set; }
    public int TotalEvasion => BaseEvasion + AdditionalEvasion;
    public int BaseMoveSpeed => Mathf.Max(Agility / Settings.AgilityPointsPerMoveSpeed, Settings.MinimalMoveSpeed);
    public int AdditionalMoveSpeed { get; set; }
    public int TotalMoveSpeed => BaseMoveSpeed + AdditionalMoveSpeed;

    public int BaseAccuracy => Mathf.Max(Focus * Settings.AccuracyPerFocusPoints, Settings.MinimalAccuracy);
    public int AdditionalAccuracy { get; set; }
    public int TotalAccuracy => BaseAccuracy + AdditionalAccuracy;
    public float BaseCritDamagePercentage => Mathf.Max(Focus * Settings.CritDamagePercentsPerFocusPoint, Settings.MinimalCritDamagePercents);
    public float AdditionalCritDamagePercentage { get; set; }
    public float TotalCritDamagePercentage => BaseCritDamagePercentage + AdditionalCritDamagePercentage;

    public int BaseInitiative => Mathf.Max(Reflex * Settings.InitiativePerReflexPoint, Settings.MinimalInitiative);
    public int AdditionalInitiative { get; set; }
    public int TotalInitiative => BaseInitiative + AdditionalInitiative;
    public float BaseCritChancePercentage => Mathf.Max(Reflex * Settings.CritChancePercentsPerReflexPoint, Settings.MinimalCritChancePercents);
    public float AdditionalCritChancePercentage { get; set; }
    public float TotalCritChancePercentage => BaseCritChancePercentage + AdditionalCritChancePercentage;

    #endregion


    //Tertiary Attributes
    public int ActionPoints = 4;
    public int MaxArmor = 1;
    public int MaxHealth = 1;
    public int MaxShield = 1;

    public CharacterArmorClass ArmorClass;


    public enum CharacterArmorClass
    {
        Light = 0,
        Medium = 1,
        Heavy = 2
    }

    public List<CombatAbility> CombatAbilities = new();
}
