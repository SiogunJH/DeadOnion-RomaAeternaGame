using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfileSettings", menuName = "Settings/CharacterProfileSettings")]
public class CharacterProfileSettings : ScriptableObject
{
    public int HealthPerVitalityPoint {  get; set; }
    public float EndurancePercentsPerVitalityPoint { get; set; }

    public int WeaponDamagePerStrengthPoint { get; set; }

    public int AbilityDamagePerPowerPoint { get; set; }

    public int EvasionPerAgilityPoint { get; set; }
    public int AgilityPointsPerMoveSpeed { get; set; }

    public int AccuracyPerFocusPoints {  get; set; }
    public float CritDamagePercentsPerFocusPoint { get; set; }

    public int InitiativePerReflexPoint {  get; set; }
    public float CritChancePercentsPerReflexPoint { get; set; }
}
