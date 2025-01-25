using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewGridMap", menuName = "Grid/Grid Map")]
public class GridMap : ScriptableObject
{
    [Header("Dimensions")]
    [SerializeField] private int _gridWidthL = 3;
    public int WidthL
    {
        get => _gridWidthL;
        set
        {
            if (_gridWidthL == value) return;
            if (!EditorUtility.IsPersistent(this)) Debug.Assert(!Application.isPlaying, GRIDMAP_DIMENSIONS_EDITED_DURING_RUNTIME_WARNING);

            _gridWidthL = value;
        }
    }

    [SerializeField] private int _gridWidthR = 3;
    public int WidthR
    {
        get => _gridWidthR;
        set
        {
            if (_gridWidthR == value) return;
            if (!EditorUtility.IsPersistent(this)) Debug.Assert(!Application.isPlaying, GRIDMAP_DIMENSIONS_EDITED_DURING_RUNTIME_WARNING);

            _gridWidthR = value;
        }
    }

    [SerializeField] private int _gridHeight = 3;
    public int Height
    {
        get => _gridHeight;
        set
        {
            if (_gridHeight == value) return;
            if (!EditorUtility.IsPersistent(this)) Debug.Assert(!Application.isPlaying, GRIDMAP_DIMENSIONS_EDITED_DURING_RUNTIME_WARNING);

            _gridHeight = value;
        }
    }

    [SerializeField, HideInInspector] public List<GridTileData> Tiles = new();

    public GridTileData this[int x, int y] { get => this[new(x, y)]; }
    public GridTileData this[Vector2 coordinates] { get => Tiles.FirstOrDefault(tile => tile.Coordinates == coordinates); }

    private const string GRIDMAP_DIMENSIONS_EDITED_DURING_RUNTIME_WARNING = "Attempted to modify GridMap's dimensions during runtime. This may have unexpected results!";

    #region Initialization

    public void InitializeGrid(bool removeUnusedTiles = false)
    {
        for (int y = 1; y <= Height; y++)
        {
            for (int x = 1; x <= WidthL; x++)
            {
                InitializeTile(x, y);
            }

            for (int x = 1; x <= WidthR; x++)
            {
                InitializeTile(-x, y);
            }
        }

        if (removeUnusedTiles) OptimizeGrid();
    }

    private void InitializeTile(int x, int y)
    {
        Vector2 index = new(x, y);

        // Check if tile already exists
        GridTileData existingTile = this[index];
        if (existingTile != null) return;

        // Add a new tile if it doesn't exist
        GridTileData newTile = new(x, y);
        Tiles.Add(newTile);
    }

    public void OptimizeGrid()
    {
        // Remove unused GridTiles
        Tiles = Tiles
            .Where(tile => tile.Y <= Height && tile.Y > 0)
            .Where(tile => (tile.X < 0 && Mathf.Abs(tile.X) <= WidthR) || (tile.X > 0 && Mathf.Abs(tile.X) <= WidthL))
            .ToList();
    }

    #endregion

    #region Pathfinding

    public bool FindPathBetween(GridEntity traveler, GridTileData start, GridTileData end, out IEnumerable<GridTileData> path)
    {
        // Validate
        foreach (var tile in new List<GridTileData>() { start, end })
        {
            if (tile.IsEnabled) continue;

            Debug.LogWarning($"Tile ({tile.X},{tile.Y}) needs to be enabled!", this);
            path = null;
            return false;
        }

        // Initialize data structures for BFS
        Queue<GridTileData> queue = new();
        Dictionary<GridTileData, GridTileData> cameFrom = new(); // Keeps track of the path

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            GridTileData current = queue.Dequeue();

            // Check if the end was reached
            if (current == end)
            {
                // Reconstruct the path
                List<GridTileData> shortestPath = new();
                for (GridTileData tile = end; tile != null; tile = cameFrom[tile])
                {
                    shortestPath.Add(tile);
                }
                shortestPath.Reverse();
                path = shortestPath;
                return true;
            }

            // Add neighbors to the queue
            foreach (GridTileData neighbour in current.Neighbours)
            {
                if (neighbour.IsEnabled && (!neighbour.IsOccupied || !traveler.OccupiesTheWholeTile) && !cameFrom.ContainsKey(neighbour)) // Ensure the neighbor is valid and not visited
                {
                    queue.Enqueue(neighbour);
                    cameFrom[neighbour] = current;
                }
            }
        }

        // No path found
        path = null;
        return false;
    }

    #endregion

}