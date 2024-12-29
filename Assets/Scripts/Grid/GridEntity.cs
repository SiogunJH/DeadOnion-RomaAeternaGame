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

    public string UserFriendlyName { get => CharacterProfile.Name; }
    public GridMap Map { get => GridManager.Instance.Grid; }

    public CharacterProfile CharacterProfile;
    public GridEntityCategory Category;

    #region MonoBehaviour

    //

    #endregion

    #region Moving

    public Coroutine MoveTo(GridTileData targetTile)
    {
        // Validate
        if (!targetTile.IsEnabled || targetTile.IsOccupied)
        {
            Debug.LogWarning("Cannot move to a tile that is Disabled or Occupied!", this);
            return null;
        }

        // Get path
        if (!Location.Map.FindPathBetween(Location, targetTile, out IEnumerable<GridTileData> path))
        {
            Debug.LogWarning($"Cannot find path to Tile ({targetTile.X},{targetTile.Y})!", this);
            return null;
        }

        // Move
        return StartCoroutine(MoveThrough(path));
    }

    private IEnumerator MoveThrough(IEnumerable<GridTileData> path)
    {
        GridTileData[] pathArray = path.ToArray();

        for (int i = 1; i < pathArray.Length; i++) // Skip the first path tile, because it's the current tile
        {
            yield return StartCoroutine(StepTo(pathArray[i]));
        }
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

}
