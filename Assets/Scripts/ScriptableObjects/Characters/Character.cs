using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
    public string Name = "NOT SET";

    public CharacterTeam Team;
    public CharacterSpeed Speed;
    public CharacterArmorClass ArmorClass;

    public int ActionPoints = 4;
    public int MaxArmor;
    public int MaxHealth;
    public int MaxShield;

    public List<CombatAbility> CombatAbilities;


    public enum CharacterTeam
    {
        Neutral = 0,
        Player = 1,
        Enemy = 2,
        Friendly = 3,
        Hazard = 4
    }
    public enum CharacterSpeed
    {
        Average = 0,
        VeryFast = 1,
        Fast = 2,
        Slow = 3,
        VerySlow = 4
    }
    public enum CharacterArmorClass
    {
        Medium = 0,
        Heavy = 1,
        Light = 2
    }
}
