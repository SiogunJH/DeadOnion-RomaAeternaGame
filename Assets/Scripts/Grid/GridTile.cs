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
    [SerializeField, HideInInspector] private List<GridEntity> _occupants = new();
    public List<GridEntity> Occupants { get => _occupants == null ? new() : _occupants; }

    //
    public bool IsOccupied { get => _occupants != null && _occupants.Any(); } // this logic might need to be expanded in the future, as some GridEntities may be allowed to co-exist on the same tile (or not, idk)

    //
    public bool AddOccupant(GridEntity occupant)
    {
        if (!IsOccupied) return false;
        if (_occupants == null) _occupants = new();

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
        if (occupants == null)
        {
            _occupants = new();
            return;
        }

        _occupants = occupants.ToList();
    }

    #endregion
}
