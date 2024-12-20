using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfileSettings", menuName = "Settings/CharacterProfileSettings")]
public class CharacterProfileSettings : ScriptableObject
{
    public int HealthPerVitalityPoint { get; set; }
    public int MinimalHealthPoints { get; set; }

    public float EndurancePercentsPerVitalityPoint { get; set; }
    public float MinimalEndurancePercents { get; set; }


    public int WeaponDamagePerStrengthPoint { get; set; }
    public int MinimalWeaponDamage { get; set; }


    public int AbilityDamagePerPowerPoint { get; set; }
    public int MinimalAbilityDamage { get; set; }


    public int EvasionPerAgilityPoint { get; set; }
    public int MinimalEvasion { get; set; }

    public int AgilityPointsPerMoveSpeed { get; set; }
    public int MinimalMoveSpeed { get; set; }


    public int AccuracyPerFocusPoints { get; set; }
    public int MinimalAccuracy { get; set; }

    public float CritDamagePercentsPerFocusPoint { get; set; }
    public float MinimalCritDamagePercents { get; set; }


    public int InitiativePerReflexPoint { get; set; }
    public int MinimalInitiative { get; set; }

    public float CritChancePercentsPerReflexPoint { get; set; }
    public float MinimalCritChancePercents { get; set; } 
}
