using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
#endif

[System.Serializable]
public class GridEntity : MonoBehaviour
{
    [HideInInspector] public GridTileData Location = null;
    [HideInInspector] public int ID = -1;

    private string _userFriendlyName = "NAME NOT SET";
    public string UserFriendlyName { get => _userFriendlyName; }
    public GridMap Map { get => GridManager.Instance.Grid; }

    public GridEntityCategory Category;
    public bool OccupiesTheWholeTile;

    private bool _isMoving = false;

    #region MonoBehaviour

    //

    #endregion

    #region Moving

    public Coroutine MoveTo(GridTileData targetTile)
    {
        // Validate
        if (_isMoving)
        {
            Debug.LogWarning("Cannot move - the entity is already moving!", this);
            return null;
        }
        if (!targetTile.IsEnabled)
        {
            Debug.LogWarning("Cannot move to a tile that is Disabled!", this);
            return null;
        }
        if (OccupiesTheWholeTile && targetTile.IsOccupied)
        {
            Debug.LogWarning("Cannot move to a tile that is Occupied!", this);
            return null;
        }

        // Get path
        if (!Location.Map.FindPathBetween(this, Location, targetTile, out IEnumerable<GridTileData> path))
        {
            Debug.LogWarning($"Cannot find path to Tile ({targetTile.X},{targetTile.Y})!", this);
            return null;
        }

        // Move
        return StartCoroutine(MoveThrough(path));
    }

    private IEnumerator MoveThrough(IEnumerable<GridTileData> path)
    {
        // Get path
        GridTileData[] pathArray = path.ToArray();

        // Flag movement
        _isMoving = true;

        // Move through
        for (int i = 1; i < pathArray.Length; i++) // Skip the first path tile, because it's the current tile
        {
            yield return StartCoroutine(StepTo(pathArray[i]));
        }

        // Unflag movement
        _isMoving = false;
    }

    private IEnumerator StepTo(GridTileData tile)
    {
        // Validate
        Debug.Assert(tile != null, "GridTileData cannot be null!", this);
        Debug.Assert(tile.Controller != null, "GridTileController cannot be null!", this);

        // Change tile
        bool removalResult = Location.RemoveOccupant(this);
        Debug.Assert(removalResult, $"GridEntity '{UserFriendlyName}' (ID: {ID}) could not be removed from Tile ({Location.X},{Location.Y})");

        Location = tile;

        bool additionResult = Location.AddOccupant(this);
        Debug.Assert(additionResult, $"GridEntity '{UserFriendlyName}' (ID: {ID}) could not be added to Tile ({Location.X},{Location.Y})");

        // Adjust position visually
        float duration = 0.45f;
        float elapsed = 0f;
        Vector3 ogPos = transform.position;
        Vector3 targetPos = tile.Controller.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // Normalized time [0, 1]
            t = 1f - Mathf.Pow(1f - t, 2f); // Ease-out: quadratic function

            transform.position = Vector3.Lerp(ogPos, targetPos, t);
            yield return null;
        }

        // Adjust final position
        transform.position = targetPos;
    }

    #endregion

#if UNITY_EDITOR

    #region DEBUG

    [Button]
    private void MoveAtRandom()
    {
        // Validate
        if (!Application.isPlaying)
        {
            Debug.LogWarning("MoveAtRandom can only be activated at runtime!");
            return;
        }
        Debug.Assert(Location != null, "GridEntity Location cannot be null!", this);

        var pool = Map.Tiles.Where(tile => tile.IsEnabled && !tile.IsOccupied && tile != Location).ToArray();

        GridTileData end = pool[Random.Range(0, pool.Length)];

        // Debug.Log($"Move from ({Location.X},{Location.Y}) to ({end.X},{end.Y})");

        MoveTo(end);
    }

    #endregion

#endif
}
