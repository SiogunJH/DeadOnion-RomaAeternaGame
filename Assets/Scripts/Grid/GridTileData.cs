using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GridTileData
{
    public GridMap Map { get => GridManager.Instance.Grid; }

    public int X { get => Coordinates.x; }
    public int Y { get => Coordinates.y; }

    public GridTileController Controller;

    public Vector2Int Coordinates;

    #region Neighbours

    public IEnumerable<GridTileData> Neighbours
    {
        get
        {
            Debug.Assert(Map != null, "Map reference is invalid!");

            List<GridTileData> neighbours = new();

            GridTileData tile = Map[X, Y + 1];
            if (tile != null) neighbours.Add(tile);

            tile = Map[X, Y - 1];
            if (tile != null) neighbours.Add(tile);

            tile = Map[X + 1, Y];
            if (tile != null) neighbours.Add(tile);

            tile = Map[X - 1, Y];
            if (tile != null) neighbours.Add(tile);

            return neighbours;
        }
    }

    #endregion

    #region Constructor

    public GridTileData(Vector2Int coordinates) : this(coordinates.x, coordinates.y) { }
    public GridTileData(int x, int y)
    {
        Coordinates = new(x, y);
    }

    #endregion

    #region ToString

    public override string ToString()
    {
        string tileString = $"Tile ({X},{Y})";
        if (Occupants.Any())
        {
            int index = 0;

            tileString += " {";
            foreach (var occupant in Occupants) tileString += $" {index++}: {occupant.UserFriendlyName} (ID: {occupant.ID}), ";
            tileString = tileString[..^2];
            tileString += " }";
        }
        return tileString;
    }

    #endregion

    #region Occupants

    //
    [SerializeField, HideInInspector] private HashSet<GridEntity> _occupants = new();
    public HashSet<GridEntity> Occupants { get => _occupants == null ? new() : _occupants; }

    //
    public bool IsOccupied { get => _occupants != null && _occupants.Any(occ => occ.OccupiesTheWholeTile); }

    //
    public bool AddOccupant(GridEntity occupant)
    {
        // Validate
        if (IsOccupied && occupant.OccupiesTheWholeTile)
        {
            Debug.LogWarning($"Cannot add '{occupant.UserFriendlyName}' to Tile ({X},{Y}) - the tile is occupied!");
            return false;
        }

        // Add
        if (_occupants == null) _occupants = new();
        _occupants.Add(occupant);

        // Adopt
        Debug.Assert(Controller != null, "GridTile is missing a Controller!");
        occupant.transform.parent = Controller.transform;

        //Results
        return true;

    }

    //
    public bool RemoveOccupant(GridEntity occupant)
    {
        // Validate
        Debug.Assert(Occupants.Any(occ => occ.ID == occupant.ID), $"GridEntity '{occupant.UserFriendlyName}' is not an occupant of GridTile ({X},{Y})");

        // Remove
        int removedCount = _occupants.RemoveWhere(occ => occ.ID == occupant.ID);
        Debug.Assert(removedCount == 0 || removedCount == 1, $"More than one occupant was removed!");

        // Orphan
        occupant.transform.parent = null;

        // Results
        return removedCount != 0;
    }

    //
    public void SetOccupants(IEnumerable<GridEntity> occupants)
    {
        // Assign
        _occupants = occupants?.ToHashSet() ?? new();

        // Reparent
        if (Controller != null)
            foreach (var occupant in _occupants)
                occupant.transform.parent = Controller.transform;
    }

    #endregion

    #region Tags

    public TileTag Tags = 0;

    public enum TileTag
    {
        None = 0,
        AllySpawn = 1 << 0,
        EnemySpawn = 1 << 1,
    }

    public bool HasTag(TileTag searchedTag)
    {
        TileTag[] matchingTags = EnumExtensions.GetMatchingFlags<TileTag>((int)Tags);
        return matchingTags.Contains(searchedTag);
    }

    #endregion
}
