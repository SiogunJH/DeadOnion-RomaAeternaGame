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
    public bool IsEnabled;
    public GridEntityCategory AllowedOccupanTypes;

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

    public GridTileData(Vector2Int coordinates, GridEntityCategory allowedOccupanTypes) : this(coordinates.x, coordinates.y, allowedOccupanTypes) { }
    public GridTileData(int x, int y, GridEntityCategory allowedOccupanTypes)
    {
        Coordinates = new(x, y);
        IsEnabled = true;
        AllowedOccupanTypes = allowedOccupanTypes;
    }

    #endregion

    #region ToString

    public override string ToString()
    {
        string tileString = $"Tile ({X},{Y})";
        if (!IsEnabled) tileString += " [Disabled]";
        if (Occupants.Any())
        {
            tileString += " {";
            for (int i = 0; i < Occupants.Count; i++) tileString += $" {i}: {Occupants[i].UserFriendlyName} (ID: {Occupants[i].ID}), ";
            tileString = tileString.Substring(0, tileString.Length - 2);
            tileString += " }";
        }
        return tileString;
    }

    #endregion

    #region Occupants

    //
    [SerializeField, HideInInspector] private List<GridEntity> _occupants = new();
    public List<GridEntity> Occupants { get => _occupants == null ? new() : _occupants; }

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
        int removedCount = _occupants.RemoveAll(occ => occ.ID == occupant.ID);
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
        _occupants = occupants?.ToList() ?? new();

        // Reparent
        if (Controller != null)
            foreach (var occupant in _occupants)
                occupant.transform.parent = Controller.transform;
    }

    #endregion
}
