using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatAbility : ScriptableObject
{
    public string Name = "NOT SET";

    [HideInInspector]
    public int Width = 0;
    [HideInInspector]
    public int Height = 0;

    [HideInInspector]
    public int ActionPointCost = 0;
    [HideInInspector]
    public bool HasCastTime = false;

    [HideInInspector]
    public CombatAbilityEffect[] AbilityEffects = new CombatAbilityEffect[0];
    [HideInInspector]
    public List<Vector2Int> Range = new();

    public TileType TargetTile = TileType.Unknown;

    #region 

    public bool CanUseAbility(Character combatant)
    {
        // Has enough action points left
        if (combatant.ActionPointsLeft < ActionPointCost)
        {
            // Debug.Log($"[CanUseAbility] {combatant.CharacterProfile.Name} has not enough action points left to use {Name}");
            return false;
        }

        // Has a valid tile target
        if (GetValidTargets(combatant).Count() == 0)
        {
            // Debug.Log($"[CanUseAbility] {combatant.CharacterProfile.Name} has no valid targets for {Name}");
            return false;
        }

        // Debug.Log($"[CanUseAbility] {combatant.CharacterProfile.Name} can use {Name}");
        return true;
    }

    public IEnumerable<GridTileData> GetValidTargets(Character combatant)
    {
        var tilesInRange = GridManager.Instance.Grid.GetTilesInPattern(combatant.Location, Range).ToList();
        // Debug.Log($"Found {tilesInRange.Count()} tiles in range for {Name}");

        HashSet<GridTileData> validTargets = new();

        if ((TargetTile & TileType.Empty) == TileType.Empty)
        {
            var emptyTiles = tilesInRange.Where(t => !t.IsOccupied);
            validTargets.UnionWith(emptyTiles);

            // Debug.Log($"Added {emptyTiles.Count()} empty tiles to Valid Tiles pool");
        }

        if ((TargetTile & TileType.Self) == TileType.Self)
        {
            var selfTiles = tilesInRange.Where(t => t.Occupants.Any(occ => occ.ID == combatant.ID));
            validTargets.UnionWith(selfTiles);

            // Debug.Log($"Added {selfTiles.Count()} tiles that contain the caster to Valid Tiles pool");
        }

        if ((TargetTile & TileType.Enemy) == TileType.Enemy)
        {
            var enemyTiles = tilesInRange.Where(t => t.Occupants.Any(occ => occ.Team != combatant.Team));
            validTargets.UnionWith(enemyTiles);

            // Debug.Log($"Added {enemyTiles.Count()} tiles that contain an enemy to Valid Tiles pool");
        }

        if ((TargetTile & TileType.Ally) == TileType.Ally)
        {
            var allyTiles = tilesInRange.Where(t => t.Occupants.Any(occ => occ.Team == combatant.Team));
            validTargets.UnionWith(allyTiles);

            // Debug.Log($"Added {allyTiles.Count()} tiles that contain an ally to Valid Tiles pool");
        }

        return validTargets;
    }

    #endregion

    #region Enums

    [System.Flags]
    public enum TileType
    {
        Unknown = 0,

        Empty = 1 << 0,
        Self = 1 << 1,
        Enemy = 1 << 2,
        Ally = 1 << 3,
    }

    #endregion

    public bool PlaysAnimation = false;
    public Sprite CasterFrame1 = null;
    public Sprite CasterFrame2 = null;
}
