using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GridTileData
{
    public GridMap Map { get => GridManager.Instance.Grid; }

    public GridTileController Controller;

    public Vector2Int Coordinates;
    public int X { get => Coordinates.x; }
    public int Y { get => Coordinates.y; }
    public bool IsEnabled;

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
        IsEnabled = true;
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
        if (IsOccupied && occupant.OccupiesTheWholeTile)
        {
            Debug.Log($"Cannot add '{occupant.UserFriendlyName}' to Tile ({X},{Y}) - the tile is occupied!");
            return false;
        }
        if (_occupants == null) _occupants = new();

        _occupants.Add(occupant);
        return true;

    }

    //
    public bool RemoveOccupant(GridEntity occupant)
    {
        Debug.Assert(Occupants.Any(occ => occ.ID == occupant.ID), $"GridEntity '{occupant.UserFriendlyName}' is not an occupant of GridTile ({X},{Y})");

        int removedCount = _occupants.RemoveAll(occ => occ.ID == occupant.ID);
        Debug.Assert(removedCount == 0 || removedCount == 1, $"More than one occupant was removed!");

        return removedCount != 0;
    }

    //
    public void SetOccupants(IEnumerable<GridEntity> occupants)
    {
        if (occupants == null)
        {
            _occupants = new();
            return;
        }

        _occupants = occupants.ToList();
    }

    #endregion
}
