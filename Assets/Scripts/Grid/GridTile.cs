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
    private List<GridEntity> _occupants = new();

    //
    public bool IsOccupied { get => _occupants.Any(); } // this logic might need to be expanded in the future, as some GridEntities may be allowed to co-exist on the same tile (or not, idk)

    //
    public List<GridEntity> GetOccupants() => _occupants;

    //
    public bool AddOccupant(GridEntity occupant)
    {
        if (!IsOccupied) return false;

        _occupants.Add(occupant);
        return true;

    }

    //
    public bool RemoveOccupant(GridEntity occupant)
    {
        return _occupants.Remove(occupant);
    }

    //
    public void SetOccupants(IEnumerable<GridEntity> occupants)
    {
        _occupants = occupants.ToList();
    }

    #endregion
}
