using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GridTile
{
    public Vector2 Coordinates;
    public bool IsEnabled;

    #region Constructor

    public GridTile(Vector2 coordinates) : this(coordinates.x, coordinates.y) { }
    public GridTile(float x, float y)
    {
        Coordinates = new(x, y);
        IsEnabled = true;
    }

    #endregion

    #region Occupants

    //
    private HashSet<GridEntity> Occupants = new();

    //
    public bool IsOccupied { get => Occupants.Any(); } // this logic might need to be expanded in the future, as some GridEntities may be allowed to co-exist on the same tile (or not, idk)

    //
    public HashSet<GridEntity> GetOccupants() => Occupants;

    //
    public bool AddOccupant(GridEntity occupant)
    {
        if (!IsOccupied) return Occupants.Add(occupant);
        return false;
    }

    //
    public bool RemoveOccupant(GridEntity occupant)
    {
        return Occupants.Remove(occupant);
    }

    #endregion
}
