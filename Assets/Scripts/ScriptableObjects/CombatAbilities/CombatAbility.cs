using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAbility : ScriptableObject
{
    public string Name = "NOT SET";
    [HideInInspector]
    public int Width = 0;
    [HideInInspector]
    public int Height = 0;

    [HideInInspector]
    public CombatAbility[] AbilityEffects;
}